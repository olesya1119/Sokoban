using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace MonoGameLibrary;

public class Core : Game
{
    internal static Core s_instance;

    /// <summary>
    /// Возвращает ссылку на экземпляр Core.
    /// </summary>
    public static Core Instance => s_instance;

    // Сцена, которая в данный момент активна.
    private static Scene s_activeScene;

    // Следующая сцена, на которую нужно переключиться (если задана).
    private static Scene s_nextScene;

    /// <summary>
    /// Возвращает GraphicsDeviceManager, который управляет параметрами вывода графики.
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    /// Возвращает GraphicsDevice, используемый для создания графических ресурсов и отрисовки примитивов.
    /// </summary>
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    /// <summary>
    /// Возвращает SpriteBatch, используемый для всей 2D-отрисовки.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    /// Возвращает ContentManager, используемый для загрузки глобальных ресурсов.
    /// </summary>
    public static new ContentManager Content { get; private set; }

    /// <summary>
    /// Возвращает ссылку на систему управления вводом.
    /// </summary>
    public static InputManager Input { get; private set; }

    /// <summary>
    /// Получает или задаёт значение, указывающее, должна ли игра завершаться при нажатии клавиши Esc.
    /// </summary>
    public static bool ExitOnEscape { get; set; }

    /// <summary>
    /// Возвращает ссылку на систему управления аудио.
    /// </summary>
    public static AudioController Audio { get; private set; }

    /// <summary>
    /// Создаёт новый экземпляр Core.
    /// </summary>
    /// <param name="title">Заголовок, отображаемый в заголовке окна игры.</param>
    /// <param name="width">Начальная ширина окна игры в пикселях.</param>
    /// <param name="height">Начальная высота окна игры в пикселях.</param>
    /// <param name="fullScreen">Указывает, должна ли игра запускаться в полноэкранном режиме.</param>
    public Core(string title, int width, int height, bool fullScreen)
    {
        // Проверяем, чтобы не было создано несколько экземпляров Core.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        // Сохраняем ссылку на движок для глобального доступа к членам.
        s_instance = this;

        // Создаём новый graphics device manager.
        Graphics = new GraphicsDeviceManager(this);

        // Устанавливаем настройки графики по умолчанию
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;

        // Применяем изменения параметров вывода графики.
        Graphics.ApplyChanges();

        // Устанавливаем заголовок окна
        Window.Title = title;

        // Устанавливаем ContentManager ядра как ссылку на ContentManager базового Game.
        Content = base.Content;

        // Устанавливаем корневую директорию для контента.
        Content.RootDirectory = "Content";

        // Курсор мыши видим по умолчанию.
        IsMouseVisible = true;

        // Выход по Esc включён по умолчанию
        ExitOnEscape = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Устанавливаем GraphicsDevice ядра как ссылку на GraphicsDevice базового Game.
        GraphicsDevice = base.GraphicsDevice;

        // Создаём экземпляр SpriteBatch.
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // Создаём менеджер ввода.
        Input = new InputManager();

        // Создаём контроллер аудио.
        Audio = new AudioController();
    }

    protected override void UnloadContent()
    {
        // Освобождаем ресурсы аудио-контроллера.
        Audio.Dispose();

        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // Обновляем менеджер ввода.
        Input.Update(gameTime);

        // Обновляем аудио-контроллер.
        Audio.Update();

        if (ExitOnEscape && Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        // Если установлена следующая сцена для переключения — выполняем переход.
        if (s_nextScene != null)
        {
            TransitionScene();
        }

        // Если есть активная сцена — обновляем её.
        if (s_activeScene != null)
        {
            s_activeScene.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Если есть активная сцена — отрисовываем её.
        if (s_activeScene != null)
        {
            s_activeScene.Draw(gameTime);
        }

        base.Draw(gameTime);
    }

    public static void ChangeScene(Scene next)
    {
        // Устанавливаем следующую сцену только если это не тот же самый экземпляр,
        // что уже активен.
        if (s_activeScene != next)
        {
            s_nextScene = next;
        }
    }

    private static void TransitionScene()
    {
        // Если есть активная сцена — освобождаем её.
        if (s_activeScene != null)
        {
            s_activeScene.Dispose();
        }

        // Принудительно запускаем сборщик мусора, чтобы гарантировать очистку памяти.
        GC.Collect();

        // Меняем текущую активную сцену на новую.
        s_activeScene = s_nextScene;

        // Обнуляем значение следующей сцены, чтобы переключение не происходило снова и снова.
        s_nextScene = null;

        // Если активная сцена теперь не null — инициализируем её.
        // Помни: как и у Game, вызов Initialize() также приводит к вызову Scene.LoadContent().
        if (s_activeScene != null)
        {
            s_activeScene.Initialize();
        }
    }
}
