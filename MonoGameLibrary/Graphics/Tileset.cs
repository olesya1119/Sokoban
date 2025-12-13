namespace MonoGameLibrary.Graphics;

public class Tileset
{
    private readonly TextureRegion[] _tiles;

    /// <summary>
    /// Возвращает ширину (в пикселях) каждой тайлы в этом наборе.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    /// Возвращает высоту (в пикселях) каждой тайлы в этом наборе.
    /// </summary>
    public int TileHeight { get; }

    /// <summary>
    /// Возвращает общее количество столбцов в этом наборе тайлов.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Возвращает общее количество строк в этом наборе тайлов.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Возвращает общее количество тайлов в этом наборе.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Создаёт новый набор тайлов (tileset) на основе указанного региона текстуры
    /// с заданной шириной и высотой тайла.
    /// </summary>
    /// <param name="textureRegion">Регион текстуры, содержащий тайлы для этого tileset.</param>
    /// <param name="tileWidth">Ширина одной тайлы в наборе.</param>
    /// <param name="tileHeight">Высота одной тайлы в наборе.</param>
    public Tileset(TextureRegion textureRegion, int tileWidth, int tileHeight)
    {
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Columns = textureRegion.Width / tileWidth;
        Rows = textureRegion.Height / tileHeight;
        Count = Columns * Rows;

        // Создаём регионы текстуры, которые образуют каждую отдельную тайлу
        _tiles = new TextureRegion[Count];

        for (int i = 0; i < Count; i++)
        {
            int x = i % Columns * tileWidth;
            int y = i / Columns * tileHeight;
            _tiles[i] = new TextureRegion(
                textureRegion.Texture,
                textureRegion.SourceRectangle.X + x,
                textureRegion.SourceRectangle.Y + y,
                tileWidth,
                tileHeight
            );
        }
    }

    /// <summary>
    /// Возвращает регион текстуры для тайлы из этого набора по указанному индексу.
    /// </summary>
    /// <param name="index">Индекс региона текстуры в этом наборе тайлов.</param>
    /// <returns>Регион текстуры тайлы из этого набора по заданному индексу.</returns>
    public TextureRegion GetTile(int index) => _tiles[index];

    /// <summary>
    /// Возвращает регион текстуры для тайлы из этого набора по её позиции (столбец, строка).
    /// </summary>
    /// <param name="column">Столбец в наборе, где находится регион текстуры.</param>
    /// <param name="row">Строка в наборе, где находится регион текстуры.</param>
    /// <returns>Регион текстуры тайлы из этого набора по указанным координатам.</returns>
    public TextureRegion GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }
}
