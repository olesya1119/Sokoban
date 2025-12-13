using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class KeyboardInfo
{
    /// <summary>
    /// Возвращает состояние клавиатурного ввода на предыдущем цикле обновления.
    /// </summary>
    public KeyboardState PreviousState { get; private set; }

    /// <summary>
    /// Возвращает состояние клавиатурного ввода на текущем цикле обновления.
    /// </summary>
    public KeyboardState CurrentState { get; private set; }

    /// <summary>
    /// Создаёт новый экземпляр KeyboardInfo.
    /// </summary>
    public KeyboardInfo()
    {
        PreviousState = new KeyboardState();
        CurrentState = Keyboard.GetState();
    }

    /// <summary>
    /// Обновляет информацию о состоянии ввода с клавиатуры.
    /// </summary>
    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }


    /// <summary>
    /// Возвращает значение, указывающее, нажата ли сейчас указанная клавиша.
    /// </summary>
    /// <param name="key">Клавиша, которую нужно проверить.</param>
    /// <returns>true, если указанная клавиша сейчас нажата; иначе false.</returns>
    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    /// <summary>
    /// Возвращает значение, указывающее, отпущена ли сейчас указанная клавиша.
    /// </summary>
    /// <param name="key">Клавиша, которую нужно проверить.</param>
    /// <returns>true, если указанная клавиша сейчас отпущена; иначе false.</returns>
    public bool IsKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key);
    }

    /// <summary>
    /// Возвращает значение, указывающее, была ли указанная клавиша нажата на текущем кадре (переход: отпущена → нажата).
    /// </summary>
    /// <param name="key">Клавиша, которую нужно проверить.</param>
    /// <returns>true, если указанная клавиша была нажата на текущем кадре; иначе false.</returns>
    public bool WasKeyJustPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    /// <summary>
    /// Возвращает значение, указывающее, была ли указанная клавиша отпущена на текущем кадре (переход: нажата → отпущена).
    /// </summary>
    /// <param name="key">Клавиша, которую нужно проверить.</param>
    /// <returns>true, если указанная клавиша была отпущена на текущем кадре; иначе false.</returns>
    public bool WasKeyJustReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }
}
