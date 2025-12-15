using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Sokoban.GameLogic;

public sealed class Level
{
    public Map Map { get; }
    public Point PlayerStart { get; }
    public List<Point> Boxes { get; }
    public HashSet<Point> Goals { get; }

    public int Width => Map.Width;
    public int Height => Map.Height;

    public Level(Map map, Point playerStart, List<Point> boxes, HashSet<Point> goals)
    {
        Map = map;
        PlayerStart = playerStart;
        Boxes = boxes;
        Goals = goals;
    }

    public bool IsVoid(Point p) => Map.IsVoid(p);
    public bool IsWalkableBase(Point p) => Map.IsWalkableBase(p);
    public bool IsWall(Point p) => Map.IsWall(p);
}


