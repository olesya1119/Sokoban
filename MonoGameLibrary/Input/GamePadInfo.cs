using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class GamePadInfo
{
    private TimeSpan _vibrationTimeRemaining = TimeSpan.Zero;

    /// <summary>
    /// Возвращает индекс игрока, для которого предназначен этот геймпад.
    /// </summary>
    public PlayerIndex PlayerIndex { get; }

    /// <summary>
    /// Возвращает состояние ввода этого геймпада на предыдущем цикле обновления.
    /// </summary>
    public GamePadState PreviousState { get; private set; }

    /// <summary>
    /// Возвращает состояние ввода этого геймпада на текущем цикле обновления.
    /// </summary>
    public GamePadState CurrentState { get; private set; }

    /// <summary>
    /// Возвращает значение, указывающее, подключён ли сейчас этот геймпад.
    /// </summary>
    public bool IsConnected => CurrentState.IsConnected;

    /// <summary>
    /// Возвращает значение левого стика этого геймпада.
    /// </summary>
    public Vector2 LeftThumbStick => CurrentState.ThumbSticks.Left;

    /// <summary>
    /// Возвращает значение правого стика этого геймпада.
    /// </summary>
    public Vector2 RightThumbStick => CurrentState.ThumbSticks.Right;

    /// <summary>
    /// Возвращает значение левого триггера этого геймпада.
    /// </summary>
    public float LeftTrigger => CurrentState.Triggers.Left;

    /// <summary>
    /// Возвращает значение правого триггера этого геймпада.
    /// </summary>
    public float RightTrigger => CurrentState.Triggers.Right;

    /// <summary>
    /// Создаёт новый GamePadInfo для геймпада, подключённого к указанному индексу игрока.
    /// </summary>
    /// <param name="playerIndex">Индекс игрока для этого геймпада.</param>
    public GamePadInfo(PlayerIndex playerIndex)
    {
        PlayerIndex = playerIndex;
        PreviousState = new GamePadState();
        CurrentState = GamePad.GetState(playerIndex);
    }

    /// <summary>
    /// Обновляет информацию о состоянии ввода для этого геймпада.
    /// </summary>
    /// <param name="gameTime">Снимок временных параметров для текущего кадра.</param>
    public void Update(GameTime gameTime)
    {
        PreviousState = CurrentState;
        CurrentState = GamePad.GetState(PlayerIndex);

        if (_vibrationTimeRemaining > TimeSpan.Zero)
        {
            _vibrationTimeRemaining -= gameTime.ElapsedGameTime;

            if (_vibrationTimeRemaining <= TimeSpan.Zero)
            {
                StopVibration();
            }
        }
    }

    /// <summary>
    /// Возвращает значение, указывающее, нажата ли сейчас указанная кнопка геймпада.
    /// </summary>
    /// <param name="button">Кнопка геймпада, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка геймпада сейчас нажата; иначе false.</returns>
    public bool IsButtonDown(Buttons button)
    {
        return CurrentState.IsButtonDown(button);
    }

    /// <summary>
    /// Возвращает значение, указывающее, отпущена ли сейчас указанная кнопка геймпада.
    /// </summary>
    /// <param name="button">Кнопка геймпада, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка геймпада сейчас отпущена; иначе false.</returns>
    public bool IsButtonUp(Buttons button)
    {
        return CurrentState.IsButtonUp(button);
    }

    /// <summary>
    /// Возвращает значение, указывающее, была ли указанная кнопка геймпада нажата на текущем кадре (переход: отпущена → нажата).
    /// </summary>
    /// <param name="button">Кнопка геймпада, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка геймпада была нажата на текущем кадре; иначе false.</returns>
    public bool WasButtonJustPressed(Buttons button)
    {
        return CurrentState.IsButtonDown(button) && PreviousState.IsButtonUp(button);
    }

    /// <summary>
    /// Возвращает значение, указывающее, была ли указанная кнопка геймпада отпущена на текущем кадре (переход: нажата → отпущена).
    /// </summary>
    /// <param name="button">Кнопка геймпада, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка геймпада была отпущена на текущем кадре; иначе false.</returns>
    public bool WasButtonJustReleased(Buttons button)
    {
        return CurrentState.IsButtonUp(button) && PreviousState.IsButtonDown(button);
    }

    /// <summary>
    /// Устанавливает вибрацию для всех моторов этого геймпада.
    /// </summary>
    /// <param name="strength">Сила вибрации от 0.0f (нет) до 1.0f (максимум).</param>
    /// <param name="time">Длительность вибрации.</param>
    public void SetVibration(float strength, TimeSpan time)
    {
        _vibrationTimeRemaining = time;
        GamePad.SetVibration(PlayerIndex, strength, strength);
    }

    /// <summary>
    /// Останавливает вибрацию всех моторов этого геймпада.
    /// </summary>
    public void StopVibration()
    {
        GamePad.SetVibration(PlayerIndex, 0.0f, 0.0f);
    }
}
