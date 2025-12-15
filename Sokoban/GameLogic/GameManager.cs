using Microsoft.Xna.Framework;
using Sokoban.GameLogic.Objects;
using System.Collections.Generic;

namespace Sokoban.GameLogic;

public sealed class GameManager
{
    private readonly Level _level;

    public Level Level => _level;

    public Player Player { get; }
    public IReadOnlyList<Box> Boxes => _boxes;
    private readonly List<Box> _boxes;

    public GameManager(Level level)
    {
        _level = level;

        Player = new Player(level.PlayerStart);

        _boxes = new List<Box>(level.Boxes.Count);
        foreach (var b in level.Boxes)
            _boxes.Add(new Box(b));
    }

    public bool IsWin()
    {
        foreach (var box in _boxes)
            if (!_level.Goals.Contains(box.Grid)) return false;
        return true;
    }

    public bool TryMove(Direction dir)
    {
        Player.SetFacing(dir);

        Point delta = dir switch
        {
            Direction.Up => new Point(0, -1),
            Direction.Down => new Point(0, 1),
            Direction.Left => new Point(-1, 0),
            Direction.Right => new Point(1, 0),
            _ => Point.Zero
        };

        if (delta == Point.Zero)
            return false;

        Point next = Player.Grid + delta;

        // нельзя в void/стену
        if (!_level.Map.IsWalkableBase(next))
            return false;

        int boxIndex = FindBoxAt(next);

        // обычный шаг
        if (boxIndex == -1)
        {
            Player.MoveTo(next); // событие Moved улетит наружу
            return true;
        }

        // толкание
        Point beyond = next + delta;

        if (!_level.Map.IsWalkableBase(beyond))
            return false;

        if (FindBoxAt(beyond) != -1)
            return false;

        // двигаем ящик и игрока
        _boxes[boxIndex].MoveTo(beyond); // событие

        _boxes[boxIndex].SetState(
             _level.Goals.Contains(_boxes[boxIndex].Grid) ? BoxState.OnGoal : BoxState.Normal
        );

        Player.MoveTo(next);             // событие
        return true;
    }

    private int FindBoxAt(Point p)
    {
        for (int i = 0; i < _boxes.Count; i++)
            if (_boxes[i].Grid == p) return i;
        return -1;
    }
}
