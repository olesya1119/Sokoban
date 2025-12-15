using Microsoft.Xna.Framework;
using System;

namespace Sokoban.GameLogic
{
    public abstract class GameObject
    {
        public Point Grid { get; private set; }

        public event Action<GameObject, Point, Point>? Moved; // Перемещение объекта из одной точки в другую

        protected GameObject(Point start)
        {
            Grid = start;
        }

        // Оповещаем о перемещении
        protected void SetGrid(Point to)
        {
            if (to == Grid) return;
            var from = Grid;
            Grid = to;
            Moved?.Invoke(this, from, to);
        }
    }
}


