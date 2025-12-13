using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Sprite
{
    /// <summary>
    /// Получает или задаёт исходный регион текстуры, который представляет этот спрайт.
    /// </summary>
    public TextureRegion Region { get; set; }

    /// <summary>
    /// Получает или задаёт цветовую маску, применяемую при отрисовке этого спрайта.
    /// </summary>
    /// <remarks>
    /// Значение по умолчанию — Color.White
    /// </remarks>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// Получает или задаёт величину вращения в радианах, применяемую при отрисовке этого спрайта.
    /// </summary>
    /// <remarks>
    /// Значение по умолчанию — 0.0f
    /// </remarks>
    public float Rotation { get; set; } = 0.0f;

    /// <summary>
    /// Получает или задаёт коэффициент масштабирования по осям X и Y при отрисовке этого спрайта.
    /// </summary>
    /// <remarks>
    /// Значение по умолчанию — Vector2.One
    /// </remarks>
    public Vector2 Scale { get; set; } = Vector2.One;

    /// <summary>
    /// Получает или задаёт точку происхождения (origin) спрайта в координатах X и Y 
    /// относительно верхнего левого угла.
    /// </summary>
    /// <remarks>
    /// Значение по умолчанию — Vector2.Zero
    /// </remarks>
    public Vector2 Origin { get; set; } = Vector2.Zero;

    /// <summary>
    /// Получает или задаёт эффекты, применяемые при отрисовке спрайта (отражение по горизонтали/вертикали).
    /// </summary>
    /// <remarks>
    /// Значение по умолчанию — SpriteEffects.None
    /// </remarks>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    /// <summary>
    /// Получает или задаёт глубину слоя, используемую при отрисовке спрайта.
    /// </summary>
    /// <remarks>
    /// Значение по умолчанию — 0.0f
    /// </remarks>
    public float LayerDepth { get; set; } = 0.0f;

    /// <summary>
    /// Возвращает ширину этого спрайта в пикселях.
    /// </summary>
    /// <remarks>
    /// Ширина вычисляется как ширина исходного региона текстуры, умноженная на масштаб по оси X.
    /// </remarks>
    public float Width => Region.Width * Scale.X;

    /// <summary>
    /// Возвращает высоту этого спрайта в пикселях.
    /// </summary>
    /// <remarks>
    /// Высота вычисляется как высота исходного региона текстуры, умноженная на масштаб по оси Y.
    /// </remarks>
    public float Height => Region.Height * Scale.Y;

    /// <summary>
    /// Создаёт новый спрайт.
    /// </summary>
    public Sprite() { }

    /// <summary>
    /// Создаёт новый спрайт, используя указанный исходный регион текстуры.
    /// </summary>
    /// <param name="region">Регион текстуры, используемый как источник для этого спрайта.</param>
    public Sprite(TextureRegion region)
    {
        Region = region;
    }

    /// <summary>
    /// Устанавливает точку происхождения (origin) спрайта в его центр.
    /// </summary>
    public void CenterOrigin()
    {
        Origin = new Vector2(Region.Width, Region.Height) * 0.5f;
    }

    /// <summary>
    /// Отправляет этот спрайт на отрисовку в текущий SpriteBatch.
    /// </summary>
    /// <param name="spriteBatch">Экземпляр SpriteBatch, используемый для пакетной отрисовки.</param>
    /// <param name="position">Координаты (X, Y), по которым нужно отрисовать спрайт.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Region.Draw(spriteBatch, position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
    }
}
