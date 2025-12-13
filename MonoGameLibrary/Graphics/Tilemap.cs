using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Tilemap
{
    private readonly Tileset _tileset;
    private readonly int[] _tiles;

    /// <summary>
    /// Возвращает общее количество строк в этом тайлмэпe.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Возвращает общее количество столбцов в этом тайлмэпe.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Возвращает общее количество тайлов в этом тайлмэпe.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Получает или задаёт коэффициент масштабирования, с которым отрисовывается каждый тайл.
    /// </summary>
    public Vector2 Scale { get; set; }

    /// <summary>
    /// Возвращает ширину (в пикселях), с которой отрисовывается каждый тайл.
    /// </summary>
    public float TileWidth => _tileset.TileWidth * Scale.X;

    /// <summary>
    /// Возвращает высоту (в пикселях), с которой отрисовывается каждый тайл.
    /// </summary>
    public float TileHeight => _tileset.TileHeight * Scale.Y;


    /// <summary>
    /// Создаёт новый тайлмэп.
    /// </summary>
    /// <param name="tileset">Набор тайлов (tileset), используемый этим тайлмэпом.</param>
    /// <param name="columns">Общее количество столбцов в этом тайлмэпe.</param>
    /// <param name="rows">Общее количество строк в этом тайлмэпe.</param>
    public Tilemap(Tileset tileset, int columns, int rows)
    {
        _tileset = tileset;
        Rows = rows;
        Columns = columns;
        Count = Columns * Rows;
        Scale = Vector2.One;
        _tiles = new int[Count];
    }

    /// <summary>
    /// Устанавливает тайл по заданному индексу в этом тайлмэпe, используя тайл
    /// из tileset с указанным идентификатором.
    /// </summary>
    /// <param name="index">Индекс тайла в этом тайлмэпe.</param>
    /// <param name="tilesetID">Идентификатор тайла в tileset, который нужно использовать.</param>
    public void SetTile(int index, int tilesetID)
    {
        _tiles[index] = tilesetID;
    }

    /// <summary>
    /// Устанавливает тайл по заданным столбцу и строке в этом тайлмэпe, используя тайл
    /// из tileset с указанным идентификатором.
    /// </summary>
    /// <param name="column">Столбец тайла в этом тайлмэпe.</param>
    /// <param name="row">Строка тайла в этом тайлмэпe.</param>
    /// <param name="tilesetID">Идентификатор тайла в tileset, который нужно использовать.</param>
    public void SetTile(int column, int row, int tilesetID)
    {
        int index = row * Columns + column;
        SetTile(index, tilesetID);
    }

    /// <summary>
    /// Возвращает регион текстуры тайла из этого тайлмэпа по заданному индексу.
    /// </summary>
    /// <param name="index">Индекс тайла в этом тайлмэпe.</param>
    /// <returns>Регион текстуры тайла по указанному индексу.</returns>
    public TextureRegion GetTile(int index)
    {
        return _tileset.GetTile(_tiles[index]);
    }

    /// <summary>
    /// Возвращает регион текстуры тайла из этого тайлмэпа по заданным
    /// столбцу и строке.
    /// </summary>
    /// <param name="column">Столбец тайла в этом тайлмэпe.</param>
    /// <param name="row">Строка тайла в этом тайлмэпe.</param>
    /// <returns>Регион текстуры тайла по указанным столбцу и строке.</returns>
    public TextureRegion GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }

    /// <summary>
    /// Отрисовывает этот тайлмэп с использованием заданного SpriteBatch.
    /// </summary>
    /// <param name="spriteBatch">SpriteBatch, используемый для отрисовки тайлмэпа.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < Count; i++)
        {
            int tilesetIndex = _tiles[i];
            TextureRegion tile = _tileset.GetTile(tilesetIndex);

            int x = i % Columns;
            int y = i / Columns;

            Vector2 position = new Vector2(x * TileWidth, y * TileHeight);
            tile.Draw(spriteBatch, position, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
        }
    }

    /// <summary>
    /// Создаёт новый тайлмэп на основе XML-файла конфигурации тайлмэпа.
    /// </summary>
    /// <param name="content">ContentManager, используемый для загрузки текстуры tileset.</param>
    /// <param name="filename">Путь к XML-файлу относительно корневой директории контента.</param>
    /// <returns>Созданный этим методом тайлмэп.</returns>
    public static Tilemap FromFile(ContentManager content, string filename)
    {
        string filePath = Path.Combine(content.RootDirectory, filename);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                // Элемент <Tileset> содержит информацию о наборе тайлов,
                // используемом этим тайлмэпом.
                //
                // Пример:
                // <Tileset region="0 0 100 100" tileWidth="10" tileHeight="10">contentPath</Tileset>
                //
                // Атрибут region представляет собой x, y, width и height
                // границы региона текстуры внутри текстуры по указанному contentPath.
                //
                // Атрибуты tileWidth и tileHeight задают ширину и
                // высоту каждой тайлы в tileset.
                //
                // Значение contentPath — это путь контента к текстуре,
                // содержащей tileset.
                XElement tilesetElement = root.Element("Tileset");

                string regionAttribute = tilesetElement.Attribute("region").Value;
                string[] split = regionAttribute.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                int width = int.Parse(split[2]);
                int height = int.Parse(split[3]);

                int tileWidth = int.Parse(tilesetElement.Attribute("tileWidth").Value);
                int tileHeight = int.Parse(tilesetElement.Attribute("tileHeight").Value);
                string contentPath = tilesetElement.Value;

                // Загружаем Texture2D по указанному пути контента
                Texture2D texture = content.Load<Texture2D>(contentPath);

                // Создаём регион текстуры на основе загруженной текстуры
                TextureRegion textureRegion = new TextureRegion(texture, x, y, width, height);

                // Создаём tileset, используя регион текстуры
                Tileset tileset = new Tileset(textureRegion, tileWidth, tileHeight);

                // Элемент <Tiles> содержит строки текста, где каждая строка
                // представляет собой один ряд в тайлмэпе. Каждая строка — это строка,
                // разделённая пробелами, где каждый элемент представляет колонку в этом ряду.
                // Значение элемента — это id тайла в tileset, который нужно отрисовать
                // в этой позиции.
                //
                // Пример:
                // <Tiles>
                //      00 01 01 02
                //      03 04 04 05
                //      03 04 04 05
                //      06 07 07 08
                // </Tiles>
                XElement tilesElement = root.Element("Tiles");

                // Разбиваем строку с данными тайлов на строки, разделяя по переводу строки
                string[] rows = tilesElement.Value.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);

                // Разбиваем первую строку, чтобы определить общее количество столбцов
                int columnCount = rows[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;

                // Создаём тайлмэп
                Tilemap tilemap = new Tilemap(tileset, columnCount, rows.Length);

                // Обрабатываем каждую строку
                for (int row = 0; row < rows.Length; row++)
                {
                    // Разбиваем строку на отдельные колонки
                    string[] columns = rows[row].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    // Обрабатываем каждую колонку текущей строки
                    for (int column = 0; column < columnCount; column++)
                    {
                        // Получаем индекс тайла из tileset для этой позиции
                        int tilesetIndex = int.Parse(columns[column]);

                        // Добавляем этот тайл в тайлмэп на позицию (row, column)
                        tilemap.SetTile(column, row, tilesetIndex);
                    }
                }

                return tilemap;
            }
        }
    }
}
