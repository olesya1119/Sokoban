using Sokoban.GameLogic;
using Sokoban;
using System;
using Microsoft.Xna.Framework;

/// <summary>
/// Класс игрока
/// </summary>
public sealed class Player : GameObject
{
    public Direction currentDir { get; private set; } = Direction.Down;

    public event Action<Direction>? DirectionChanged;

    public Player(Point start) : base(start) { }

    /// <summary>
    /// Смена направления и перемещение
    /// </summary>
    /// <param name="dir">Направление</param>
    public void SetFacing(Direction dir)
    {
        if (currentDir == dir) return;
        currentDir = dir;
        DirectionChanged?.Invoke(dir);
    }

    public void MoveTo(Point to) => SetGrid(to);
}