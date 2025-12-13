using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Input;

public class InputManager
{
    /// <summary>
    /// Возвращает информацию о состоянии ввода с клавиатуры.
    /// </summary>
    public KeyboardInfo Keyboard { get; private set; }

    /// <summary>
    /// Возвращает информацию о состоянии ввода с мыши.
    /// </summary>
    public MouseInfo Mouse { get; private set; }

    /// <summary>
    /// Возвращает информацию о состоянии геймпадов.
    /// </summary>
    public GamePadInfo[] GamePads { get; private set; }

    /// <summary>
    /// Создаёт новый InputManager.
    /// </summary>
    public InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            GamePads[i] = new GamePadInfo((PlayerIndex)i);
        }
    }

    /// <summary>
    /// Обновляет информацию о состоянии ввода с клавиатуры, мыши и геймпадов.
    /// </summary>
    /// <param name="gameTime">Снимок временных параметров для текущего кадра.</param>
    public void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }
}
