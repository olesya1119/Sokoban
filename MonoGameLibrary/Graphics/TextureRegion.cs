using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

/// <summary>
/// Представляет прямоугольную область внутри текстуры.
/// </summary>
public class TextureRegion
{
    /// <summary>
    /// Получает или задаёт исходную текстуру, частью которой является данный регион.
    /// </summary>
    public Texture2D Texture { get; set; }

    /// <summary>
    /// Получает или задаёт прямоугольную область внутри исходной текстуры.
    /// </summary>
    public Rectangle SourceRectangle { get; set; }

    /// <summary>
    /// Получает ширину региона текстуры в пикселях.
    /// </summary>
    public int Width => SourceRectangle.Width;

    /// <summary>
    /// Получает высоту региона текстуры в пикселях.
    /// </summary>
    public int Height => SourceRectangle.Height;

    /// <summary>
    /// Создаёт новый регион текстуры.
    /// </summary>
    public TextureRegion() { }

    /// <summary>
    /// Создаёт новый регион текстуры, используя указанную исходную текстуру.
    /// </summary>
    /// <param name="texture">Текстура, используемая как источник для этого региона.</param>
    /// <param name="x">Координата X верхнего левого угла региона относительно верхнего левого угла исходной текстуры.</param>
    /// <param name="y">Координата Y верхнего левого угла региона относительно верхнего левого угла исходной текстуры.</param>
    /// <param name="width">Ширина региона в пикселях.</param>
    /// <param name="height">Высота региона в пикселях.</param>
    public TextureRegion(Texture2D texture, int x, int y, int width, int height)
    {
        Texture = texture;
        SourceRectangle = new Rectangle(x, y, width, height);
    }

    /// <summary>
    /// Отправляет данный регион текстуры на отрисовку в текущем SpriteBatch.
    /// </summary>
    /// <param name="spriteBatch">Экземпляр SpriteBatch, используемый для пакетной отрисовки.</param>
    /// <param name="position">Координаты на экране, где нужно отрисовать данный регион.</param>
    /// <param name="color">Цветовая маска, применяемая при отрисовке региона.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        Draw(spriteBatch, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    /// <summary>
    /// Отправляет данный регион текстуры на отрисовку в текущем SpriteBatch.
    /// </summary>
    /// <param name="spriteBatch">Экземпляр SpriteBatch, используемый для пакетной отрисовки.</param>
    /// <param name="position">Координаты на экране для отрисовки региона.</param>
    /// <param name="color">Цветовая маска.</param>
    /// <param name="rotation">Величина вращения в радианах.</param>
    /// <param name="origin">Точка вращения/масштабирования.</param>
    /// <param name="scale">Масштаб (одинаковый по осям X и Y).</param>
    /// <param name="effects">Флаги отражения по горизонтали/вертикали.</param>
    /// <param name="layerDepth">Глубина слоя при отрисовке.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
    {
        Draw(
            spriteBatch,
            position,
            color,
            rotation,
            origin,
            new Vector2(scale, scale),
            effects,
            layerDepth
        );
    }

    /// <summary>
    /// Отправляет данный регион текстуры на отрисовку в текущем SpriteBatch.
    /// </summary>
    /// <param name="spriteBatch">Экземпляр SpriteBatch, используемый для пакетной отрисовки.</param>
    /// <param name="position">Координаты на экране для отрисовки региона.</param>
    /// <param name="color">Цветовая маска.</param>
    /// <param name="rotation">Величина вращения в радианах.</param>
    /// <param name="origin">Точка вращения/масштабирования.</param>
    /// <param name="scale">Масштаб по осям X и Y.</param>
    /// <param name="effects">Флаги отражения изображения.</param>
    /// <param name="layerDepth">Глубина слоя при отрисовке.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        spriteBatch.Draw(
            Texture,
            position,
            SourceRectangle,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );
    }
}
