using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace Sokoban.View
{
    public abstract class Animator
    {
        protected Vector2 Position;

        protected Vector2 _from;
        protected Vector2 _to;

        private float _t;
        private int _durationMs;

        public bool IsPlaying { get; private set; }

        protected Animator(Vector2 startPos)
        {
            Position = startPos;
            _from = startPos;
            _to = startPos;
            _t = 1f;
            _durationMs = Constans.AnimationTimeMS;
            IsPlaying = false;

            LoadContent();
        }

        public void MoveTo(Vector2 newPos)
        {
            _from = Position;
            _to = newPos;
            _t = 0f;
            _durationMs = Constans.AnimationTimeMS;
            IsPlaying = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsPlaying) return;

            float dtMs = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _t += dtMs / _durationMs;

            if (_t >= 1f)
            {
                _t = 1f;
                IsPlaying = false;
                Position = _to;
                return;
            }

            Position = Vector2.Lerp(_from, _to, _t);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GetSprite().Draw(spriteBatch, Position);
        }

        protected abstract Sprite GetSprite();
        protected abstract void LoadContent();
    }
}
