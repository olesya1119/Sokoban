using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class MouseInfo
{
    /// <summary>
    /// Состояние мыши на предыдущем цикле обновления.
    /// </summary>
    public MouseState PreviousState { get; private set; }

    /// <summary>
    /// Состояние мыши на текущем цикле обновления.
    /// </summary>
    public MouseState CurrentState { get; private set; }

    /// <summary>
    /// Получает или задаёт текущую позицию курсора мыши в экранных координатах.
    /// </summary>
    public Point Position
    {
        get => CurrentState.Position;
        set => SetPosition(value.X, value.Y);
    }

    /// <summary>
    /// Получает или задаёт текущую координату X курсора мыши в экранных координатах.
    /// </summary>
    public int X
    {
        get => CurrentState.X;
        set => SetPosition(value, CurrentState.Y);
    }

    /// <summary>
    /// Получает или задаёт текущую координату Y курсора мыши в экранных координатах.
    /// </summary>
    public int Y
    {
        get => CurrentState.Y;
        set => SetPosition(CurrentState.X, value);
    }

    /// <summary>
    /// Возвращает разницу позиции курсора мыши между предыдущим и текущим кадром.
    /// </summary>
    public Point PositionDelta => CurrentState.Position - PreviousState.Position;

    /// <summary>
    /// Возвращает разницу координаты X курсора мыши между предыдущим и текущим кадром.
    /// </summary>
    public int XDelta => CurrentState.X - PreviousState.X;

    /// <summary>
    /// Возвращает разницу координаты Y курсора мыши между предыдущим и текущим кадром.
    /// </summary>
    public int YDelta => CurrentState.Y - PreviousState.Y;

    /// <summary>
    /// Возвращает значение, указывающее, перемещался ли курсор мыши между предыдущим и текущим кадром.
    /// </summary>
    public bool WasMoved => PositionDelta != Point.Zero;

    /// <summary>
    /// Возвращает накопленное значение колеса прокрутки мыши с начала игры.
    /// </summary>
    public int ScrollWheel => CurrentState.ScrollWheelValue;

    /// <summary>
    /// Возвращает изменение значения колеса прокрутки между предыдущим и текущим кадром.
    /// </summary>
    public int ScrollWheelDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;


    /// <summary>
    /// Создаёт новый экземпляр MouseInfo.
    /// </summary>
    public MouseInfo()
    {
        PreviousState = new MouseState();
        CurrentState = Mouse.GetState();
    }


    /// <summary>
    /// Обновляет информацию о состоянии ввода мыши.
    /// </summary>
    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Mouse.GetState();
    }

    /// <summary>
    /// Возвращает значение, указывающее, нажата ли сейчас указанная кнопка мыши.
    /// </summary>
    /// <param name="button">Кнопка мыши, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка мыши сейчас нажата; иначе false.</returns>
    public bool IsButtonDown(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    /// <summary>
    /// Возвращает значение, указывающее, отпущена ли сейчас указанная кнопка мыши.
    /// </summary>
    /// <param name="button">Кнопка мыши, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка мыши сейчас отпущена; иначе false.</returns>
    public bool IsButtonUp(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    /// <summary>
    /// Возвращает значение, указывающее, была ли указанная кнопка мыши нажата на текущем кадре (переход: отпущена → нажата).
    /// </summary>
    /// <param name="button">Кнопка мыши, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка мыши была нажата на текущем кадре; иначе false.</returns>
    public bool WasButtonJustPressed(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed && PreviousState.XButton1 == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Pressed && PreviousState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    /// <summary>
    /// Возвращает значение, указывающее, была ли указанная кнопка мыши отпущена на текущем кадре (переход: нажата → отпущена).
    /// </summary>
    /// <param name="button">Кнопка мыши, которую нужно проверить.</param>
    /// <returns>true, если указанная кнопка мыши была отпущена на текущем кадре; иначе false.</returns>
    public bool WasButtonJustReleased(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released && PreviousState.XButton1 == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Released && PreviousState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }


    /// <summary>
    /// Устанавливает текущую позицию курсора мыши в экранных координатах и обновляет CurrentState с новой позицией.
    /// </summary>
    /// <param name="x">Координата X курсора мыши в экранных координатах.</param>
    /// <param name="y">Координата Y курсора мыши в экранных координатах.</param>
    public void SetPosition(int x, int y)
    {
        Mouse.SetPosition(x, y);
        CurrentState = new MouseState(
            x,
            y,
            CurrentState.ScrollWheelValue,
            CurrentState.LeftButton,
            CurrentState.MiddleButton,
            CurrentState.RightButton,
            CurrentState.XButton1,
            CurrentState.XButton2
        );
    }
}
