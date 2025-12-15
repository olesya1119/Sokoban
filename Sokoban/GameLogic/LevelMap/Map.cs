using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace Sokoban.GameLogic;

public class Map
{
    public const int MaxWidth = 18;
    public const int MaxHeight = 10;

    private readonly CellType[,] _cells;

    public int Width { get; }
    public int Height { get; }

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new CellType[Width, Height];
    }

    public CellType GetCell(int x, int y) => _cells[x, y];
    public CellType GetCell(Point p) => _cells[p.X, p.Y];

    public void SetCell(int x, int y, CellType type) => _cells[x, y] = type;
    public void SetCell(Point p, CellType type) => _cells[p.X, p.Y] = type;

    public bool InBounds(Point p) => p.X >= 0 && p.Y >= 0 && p.X < Width && p.Y < Height;

    public bool IsVoid(Point p) => !InBounds(p) || GetCell(p) == CellType.Void;
    public bool IsWall(Point p) => InBounds(p) && GetCell(p) == CellType.Wall;
    public bool IsGoal(Point p) => InBounds(p) && GetCell(p) == CellType.Goal;

    public bool IsWalkableBase(Point p)
    {
        if (!InBounds(p)) return false;
        var c = GetCell(p);
        return c != CellType.Void && c != CellType.Wall;
    }

    public IEnumerable<Point> EnumerateAll()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return new Point(x, y);
    }
}
