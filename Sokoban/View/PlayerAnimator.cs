using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;
using System.Collections.Generic;

namespace Sokoban.View
{
    public sealed class PlayerAnimator : Animator
    {
        private readonly Dictionary<Direction, AnimatedSprite> _walk = new();
        private readonly Dictionary<Direction, Sprite> _stay = new();

        private Direction _lastDirection = Direction.Down;

        public PlayerAnimator(Vector2 startPos) : base(startPos)
        {
        }

        private Direction CurrentDirection
        {
            get
            {
                Vector2 d = _to - _from;

                if (d == Vector2.Zero)
                    return _lastDirection;

                Direction dir;
                if (Math.Abs(d.X) > Math.Abs(d.Y))
                    dir = d.X < 0 ? Direction.Left : Direction.Right;
                else
                    dir = d.Y < 0 ? Direction.Up : Direction.Down;

                _lastDirection = dir;
                return dir;
            }
        }

        protected override void LoadContent()
        {
            var atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

            _walk[Direction.Left] = atlas.CreateAnimatedSprite("player-walk-left");
            _walk[Direction.Right] = atlas.CreateAnimatedSprite("player-walk-right");
            _walk[Direction.Up] = atlas.CreateAnimatedSprite("player-walk-up");
            _walk[Direction.Down] = atlas.CreateAnimatedSprite("player-walk-down");

            _stay[Direction.Left] = atlas.CreateSprite("player-stay-left");
            _stay[Direction.Right] = atlas.CreateSprite("player-stay-right");
            _stay[Direction.Up] = atlas.CreateSprite("player-stay-up");
            _stay[Direction.Down] = atlas.CreateSprite("player-stay-down");

            foreach (var s in _stay.Values)
                s.CenterOrigin();

            foreach (var a in _walk.Values)
                CenterAnimated(a);
        }

        private static void CenterAnimated(AnimatedSprite a)
        {
            if (a?.Region == null) return;
            a.Origin = new Vector2(a.Region.Width, a.Region.Height) * 0.5f;
        }

        protected override Sprite GetSprite()
        {
            var dir = CurrentDirection;
            return IsPlaying ? _walk[dir] : _stay[_lastDirection];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsPlaying) return;
            _walk[CurrentDirection].Update(gameTime);
        }
    }
}
