using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Scenes;

public abstract class Scene : IDisposable
{
    /// <summary>
    /// Возвращает ContentManager, используемый для загрузки ресурсов, относящихся к конкретной сцене.
    /// </summary>
    /// <remarks>
    /// Ресурсы, загруженные через этот ContentManager, будут автоматически выгружены при завершении сцены.
    /// </remarks>
    protected ContentManager Content { get; }

    /// <summary>
    /// Возвращает значение, указывающее, была ли сцена освобождена (Dispose).
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Создаёт новый экземпляр сцены.
    /// </summary>
    public Scene()
    {
        // Создаём ContentManager для сцены
        Content = new ContentManager(Core.Content.ServiceProvider);

        // Устанавливаем корневую директорию контента такой же, как и у контента игры.
        Content.RootDirectory = Core.Content.RootDirectory;
    }

    // Финализатор, вызывается при очистке объекта сборщиком мусора.
    ~Scene() => Dispose(false);

    /// <summary>
    /// Инициализирует сцену.
    /// </summary>
    /// <remarks>
    /// При переопределении в классе-наследнике убедись, что base.Initialize()
    /// всё равно вызывается, так как именно здесь вызывается LoadContent().
    /// </remarks>
    public virtual void Initialize()
    {
        LoadContent();
    }

    /// <summary>
    /// Переопредели этот метод, чтобы реализовать загрузку контента для сцены.
    /// </summary>
    public virtual void LoadContent() { }

    /// <summary>
    /// Выгружает контент, относящийся к сцене.
    /// </summary>
    public virtual void UnloadContent()
    {
        Content.Unload();
    }

    /// <summary>
    /// Обновляет сцену.
    /// </summary>
    /// <param name="gameTime">Снимок временных параметров для текущего кадра.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Отрисовывает сцену.
    /// </summary>
    /// <param name="gameTime">Снимок временных параметров для текущего кадра.</param>
    public virtual void Draw(GameTime gameTime) { }

    /// <summary>
    /// Освобождает ресурсы сцены.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Освобождает ресурсы сцены.
    /// </summary>
    /// <param name="disposing">
    /// Указывает, нужно ли освобождать управляемые ресурсы.
    /// Это значение равно true только при вызове из основного метода Dispose().
    /// При вызове из финализатора (finalizer) оно будет false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            UnloadContent();
            Content.Dispose();
        }
        IsDisposed = true;
    }
}
