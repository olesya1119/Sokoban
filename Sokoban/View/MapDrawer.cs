using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Sokoban.GameLogic;

namespace Sokoban.View;

public sealed class MapDrawer
{
    private readonly Map _map;
    private readonly int _tileSize;
    private readonly Vector2 _offset;

    private TextureAtlas _atlas;

    private Sprite _floorSprite;
    private Sprite _wallSprite;
    private Sprite _goalSprite;

    public MapDrawer(Map map, int tileSize, Vector2 offset)
    {
        _map = map;
        _tileSize = tileSize;
        _offset = offset;

        LoadContent();
    }

    private void LoadContent()
    {
        _atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        _floorSprite = _atlas.CreateSprite("floor");
        _floorSprite.CenterOrigin();

        _wallSprite = _atlas.CreateSprite("wall");
        _wallSprite.CenterOrigin();

        _goalSprite = _atlas.CreateSprite("goal");
        _goalSprite.CenterOrigin();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int y = 0; y < _map.Height; y++)
        {
            for (int x = 0; x < _map.Width; x++)
            {
                var p = new Point(x, y);

                if (_map.IsVoid(p))
                    continue;

                _floorSprite.Draw(spriteBatch, GridToWorld(p));

                if (_map.IsGoal(p))
                    _goalSprite.Draw(spriteBatch, GridToWorld(p));

                if (_map.IsWall(p))
                    _wallSprite.Draw(spriteBatch, GridToWorld(p));
            }
        }
    }

    private Vector2 GridToWorld(Point g)
        => _offset + new Vector2(g.X * _tileSize, g.Y * _tileSize);
}
