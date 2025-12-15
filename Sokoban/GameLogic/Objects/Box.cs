using Microsoft.Xna.Framework;
using Sokoban.GameLogic.Objects;
using System;

namespace Sokoban.GameLogic;

public sealed class Box : GameObject
{
    public BoxState State { get; private set; } = BoxState.Normal;

    public event Action<Box, BoxState, BoxState>? StateChanged; // смена состояния коробки

    public Box(Point start) : base(start) { }

    internal void MoveTo(Point to) => SetGrid(to);

    internal void SetState(BoxState newState)
    {
        if (State == newState) return;
        var old = State;
        State = newState;
        StateChanged?.Invoke(this, old, newState);
    }
}