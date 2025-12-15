using Microsoft.Xna.Framework;

namespace Sokoban.GameLogic
{
    public sealed class Wall
    {
        public Point Grid { get; }
        public Wall(Point grid) { Grid = grid; }
    }
}
