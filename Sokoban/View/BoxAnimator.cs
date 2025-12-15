using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Sokoban.GameLogic.Objects;
using System.Collections.Generic;

namespace Sokoban.View
{
    public sealed class BoxAnimator : Animator
    {
        private readonly Dictionary<BoxState, Sprite> _sprites = new();


        public BoxState State { get; set; } = BoxState.Normal;

        public BoxAnimator(Vector2 startPos) : base(startPos)
        {
        }

        protected override void LoadContent()
        {
            var atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

            var normal = atlas.CreateSprite("box");
            var onGoal = atlas.CreateSprite("box-in-goal");

            normal.CenterOrigin();
            onGoal.CenterOrigin();

            _sprites[BoxState.Normal] = normal;
            _sprites[BoxState.OnGoal] = onGoal;
        }

        protected override Sprite GetSprite()
        {
            if (_sprites.TryGetValue(State, out var sprite))
                return sprite;

            return _sprites[BoxState.Normal];
        }
    }

}
