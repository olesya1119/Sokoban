using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

public class Animation
{
    /// <summary>
    /// Набор регионов текстуры, составляющих кадры этой анимации.
    /// Порядок регионов в коллекции — это порядок отображения кадров.
    /// </summary>
    public List<TextureRegion> Frames { get; set; }

    /// <summary>
    /// Время задержки между кадрами перед переходом к следующему кадру анимации.
    /// </summary>
    public TimeSpan Delay { get; set; }

    /// <summary>
    /// Создаёт новую анимацию.
    /// </summary>
    public Animation()
    {
        Frames = new List<TextureRegion>();
        Delay = TimeSpan.FromMilliseconds(100);
    }

    /// <summary>
    /// Создаёт новую анимацию с указанными кадрами и задержкой.
    /// </summary>
    /// <param name="frames">Упорядоченная коллекция кадров для этой анимации.</param>
    /// <param name="delay">Время задержки между каждым кадром анимации.</param>
    public Animation(List<TextureRegion> frames, TimeSpan delay)
    {
        Frames = frames;
        Delay = delay;
    }
}
