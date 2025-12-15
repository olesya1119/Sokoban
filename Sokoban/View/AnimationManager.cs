using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sokoban.View
{
    public class AnimationManager
    {
        private readonly List<Animator> _animators = new();

        public void AddChild(Animator animator) => _animators.Add(animator);

        public void Update(GameTime gameTime)
        {
            foreach (var a in _animators)
                a.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var a in _animators)
                a.Draw(spriteBatch);
        }

        public bool IsPlayerPlaying()
        {
            foreach (var a in _animators)
                if (a is PlayerAnimator && a.IsPlaying) return true;
            return false;
        }
    }
}
