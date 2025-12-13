using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class TextureAtlas
{
    private Dictionary<string, TextureRegion> _regions;

    /// <summary>
    /// Получает или задаёт исходную текстуру, которую представляет этот атлас.
    /// </summary>
    public Texture2D Texture { get; set; }

    /// <summary>
    /// Создаёт новый атлас текстур.
    /// </summary>
    public TextureAtlas()
    {
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    /// <summary>
    /// Создаёт новый экземпляр атласа текстур, используя указанную текстуру.
    /// </summary>
    /// <param name="texture">Исходная текстура, которую представляет данный атлас.</param>
    public TextureAtlas(Texture2D texture)
    {
        Texture = texture;
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }


    /// <summary>
    /// Создаёт новый регион и добавляет его в этот атлас текстур.
    /// </summary>
    /// <param name="name">Имя, которое будет присвоено региону текстуры.</param>
    /// <param name="x">Координата X верхнего левого угла региона относительно верхнего левого угла исходной текстуры.</param>
    /// <param name="y">Координата Y верхнего левого угла региона относительно верхнего левого угла исходной текстуры.</param>
    /// <param name="width">Ширина региона в пикселях.</param>
    /// <param name="height">Высота региона в пикселях.</param>
    public void AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion region = new TextureRegion(Texture, x, y, width, height);
        _regions.Add(name, region);
    }

    /// <summary>
    /// Возвращает регион из этого атласа текстур по указанному имени.
    /// </summary>
    /// <param name="name">Имя региона, который нужно получить.</param>
    /// <returns>Экземпляр TextureRegion с указанным именем.</returns>
    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }

    /// <summary>
    /// Удаляет регион из этого атласа текстур по указанному имени.
    /// </summary>
    /// <param name="name">Имя региона, который нужно удалить.</param>
    /// <returns>true, если регион был успешно удалён; иначе false.</returns>
    public bool RemoveRegion(string name)
    {
        return _regions.Remove(name);
    }

    /// <summary>
    /// Удаляет все регионы из этого атласа текстур.
    /// </summary>
    public void Clear()
    {
        _regions.Clear();
    }

    /// <summary>
    /// Создаёт новый атлас текстур на основе XML-файла конфигурации атласа.
    /// </summary>
    /// <param name="content">ContentManager, используемый для загрузки текстуры атласа.</param>
    /// <param name="fileName">Путь к XML-файлу относительно корневой директории контента.</param>
    /// <returns>Атлас текстур, созданный этим методом.</returns>
    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        TextureAtlas atlas = new TextureAtlas();

        string filePath = Path.Combine(content.RootDirectory, fileName);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                // Элемент <Texture> содержит путь контента к Texture2D, которую нужно загрузить.
                // Мы считываем это значение и используем ContentManager для загрузки текстуры.
                string texturePath = root.Element("Texture").Value;
                atlas.Texture = content.Load<Texture2D>(texturePath);

                // Элемент <Regions> содержит отдельные элементы <Region>, каждый из которых описывает
                // отдельный регион текстуры внутри атласа.
                //
                // Пример:
                // <Regions>
                //      <Region name="spriteOne" x="0" y="0" width="32" height="32" />
                //      <Region name="spriteTwo" x="32" y="0" width="32" height="32" />
                // </Regions>
                //
                // Мы получаем все элементы <Region>, проходим по каждому
                // и создаём для него новый экземпляр TextureRegion, добавляя его в этот атлас.
                var regions = root.Element("Regions")?.Elements("Region");

                if (regions != null)
                {
                    foreach (var region in regions)
                    {
                        string name = region.Attribute("name")?.Value;
                        int x = int.Parse(region.Attribute("x")?.Value ?? "0");
                        int y = int.Parse(region.Attribute("y")?.Value ?? "0");
                        int width = int.Parse(region.Attribute("width")?.Value ?? "0");
                        int height = int.Parse(region.Attribute("height")?.Value ?? "0");

                        if (!string.IsNullOrEmpty(name))
                        {
                            atlas.AddRegion(name, x, y, width, height);
                        }
                    }
                }

                // Элемент <Animations> содержит отдельные элементы <Animation>, каждый из которых
                // описывает анимацию внутри атласа.
                //
                // Пример:
                // <Animations>
                //      <Animation name="animation" delay="100">
                //          <Frame region="spriteOne" />
                //          <Frame region="spriteTwo" />
                //      </Animation>
                // </Animations>
                //
                // Мы получаем все элементы <Animation>, проходим по каждому,
                // создаём для него экземпляр Animation и добавляем его в этот атлас.
                var animationElements = root.Element("Animations").Elements("Animation");

                if (animationElements != null)
                {
                    foreach (var animationElement in animationElements)
                    {
                        string name = animationElement.Attribute("name")?.Value;
                        float delayInMilliseconds = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");
                        TimeSpan delay = TimeSpan.FromMilliseconds(delayInMilliseconds);

                        List<TextureRegion> frames = new List<TextureRegion>();

                        var frameElements = animationElement.Elements("Frame");

                        if (frameElements != null)
                        {
                            foreach (var frameElement in frameElements)
                            {
                                string regionName = frameElement.Attribute("region").Value;
                                TextureRegion region = atlas.GetRegion(regionName);
                                frames.Add(region);
                            }
                        }

                        Animation animation = new Animation(frames, delay);
                        atlas.AddAnimation(name, animation);
                    }
                }

                return atlas;
            }
        }
    }


    /// <summary>
    /// Создаёт новый спрайт, используя регион из этого атласа текстур с указанным именем.
    /// </summary>
    /// <param name="regionName">Имя региона, на основе которого нужно создать спрайт.</param>
    /// <returns>Новый Sprite, использующий регион текстуры с указанным именем.</returns>
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        return new Sprite(region);
    }

    // Хранит анимации, добавленные в этот атлас.
    private Dictionary<string, Animation> _animations;

    /// <summary>
    /// Добавляет указанную анимацию в этот атлас текстур под заданным именем.
    /// </summary>
    /// <param name="animationName">Имя анимации.</param>
    /// <param name="animation">Анимация, которую нужно добавить.</param>
    public void AddAnimation(string animationName, Animation animation)
    {
        _animations.Add(animationName, animation);
    }

    /// <summary>
    /// Возвращает анимацию из этого атласа текстур по указанному имени.
    /// </summary>
    /// <param name="animationName">Имя анимации, которую нужно получить.</param>
    /// <returns>Анимация с указанным именем.</returns>
    public Animation GetAnimation(string animationName)
    {
        return _animations[animationName];
    }

    /// <summary>
    /// Удаляет анимацию с указанным именем из этого атласа текстур.
    /// </summary>
    /// <param name="animationName">Имя анимации, которую нужно удалить.</param>
    /// <returns>true, если анимация успешно удалена; иначе false.</returns>
    public bool RemoveAnimation(string animationName)
    {
        return _animations.Remove(animationName);
    }


    /// <summary>
    /// Создаёт новый анимированный спрайт, используя анимацию из этого атласа текстур с указанным именем.
    /// </summary>
    /// <param name="animationName">Имя анимации, которую нужно использовать.</param>
    /// <returns>Новый AnimatedSprite, использующий анимацию с указанным именем.</returns>
    public AnimatedSprite CreateAnimatedSprite(string animationName)
    {
        Animation animation = GetAnimation(animationName);
        return new AnimatedSprite(animation);
    }
}
