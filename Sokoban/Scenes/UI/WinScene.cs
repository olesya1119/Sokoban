using MonoGameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sokoban.Scenes.UI
{
    public sealed class WinScene : InterfaceScene
    {
        private readonly int _moves;
        private readonly int _levelNumber;

        private SpriteFont _font;

        public WinScene(int moves, int levelNumber) : base()
        {
            _moves = moves;
            _levelNumber = levelNumber;

            AddButton("Back to Levels", 100, -10, 110, 100, HandleBackClicked);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _font = Core.Content.Load<SpriteFont>("fonts/ui");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (_font != null)
            {
                var vp = Core.GraphicsDevice.Viewport;

                string title = "LEVEL COMPLETE!";
                string line1 = $"Level: {_levelNumber}";
                string line2 = $"Moves: {_moves}";

                DrawCentered(_font, title, new Vector2(vp.Width / 2f, vp.Height / 2f - 120));
                DrawCentered(_font, line1, new Vector2(vp.Width / 2f, vp.Height / 2f - 60));
                DrawCentered(_font, line2, new Vector2(vp.Width / 2f, vp.Height / 2f - 10));
            }

            Core.SpriteBatch.End();
        }

        private void DrawCentered(SpriteFont font, string text, Vector2 center)
        {
            Vector2 size = font.MeasureString(text);
            Core.SpriteBatch.DrawString(font, text, center - size * 0.5f, Color.White);
        }

        private void HandleBackClicked(object sender, EventArgs e)
        {
            Core.ChangeScene(new LevelMenuScene());
        }
    }
}
