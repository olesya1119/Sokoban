using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using Sokoban.GameLogic;
using Sokoban.GameLogic.Objects;
using Sokoban.Scenes.UI;
using Sokoban.View;
using System.Collections.Generic;

namespace Sokoban.Scenes;

public class GameScene : Scene
{
    public int LevelNumber { get; private set; }

    private const int TileSize = 64;

    private Level _level;
    private GameManager _game;

    private AnimationManager _animMgr;
    private MapDrawer _mapDrawer;

    private Vector2 _offset;

    public GameScene(int levelNumber)
    {
        LevelNumber = levelNumber;
        _level = LevelLoader.FromXml(Content, $"levels/level{levelNumber}.xml");
    }

    // привязка объектов игры к их аниматорам
    private readonly Dictionary<GameObject, Animator> _animByObj = new();

    public override void LoadContent()
    {
        
        _game = new GameManager(_level);

        var vp = Core.GraphicsDevice.Viewport;
        _offset = new Vector2(
            (vp.Width - _level.Map.Width * TileSize) * 0.5f,
            (vp.Height - _level.Map.Height * TileSize) * 0.5f
        );
        if (_offset.X < 0) _offset.X = 0;
        if (_offset.Y < 0) _offset.Y = 0;

        _mapDrawer = new MapDrawer(_level.Map, TileSize, _offset);

        _animMgr = new AnimationManager();


        var playerAnim = new PlayerAnimator(GridToWorld(_game.Player.Grid));
        _animMgr.AddChild(playerAnim);
        _animByObj[_game.Player] = playerAnim;

        // подписки на события игрока
        _game.Player.Moved += OnObjectMoved;

        // подписки на события коробки
        foreach (var box in _game.Boxes)
        {
            var boxAnim = new BoxAnimator(GridToWorld(box.Grid));
            _animMgr.AddChild(boxAnim);
            _animByObj[box] = boxAnim;

            box.Moved += OnObjectMoved;
            box.StateChanged += OnBoxStateChanged;
        }
    }

    public override void Update(GameTime gameTime)
    {
        _animMgr.Update(gameTime);

        // ввод блокируем пока любые анимации идут
        if (_animMgr.IsPlayerPlaying())
            return;

        if (_game.IsWin())
        {
            Core.ChangeScene(new WinScene(_game.MovesCount, LevelNumber));
            return;
        }


        if (!TryGetDirectionJustPressed(out var dir))
            return;

        _game.TryMove(dir);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _mapDrawer.Draw(Core.SpriteBatch);
        _animMgr.Draw(Core.SpriteBatch);

        Core.SpriteBatch.End();
    }

    private void OnObjectMoved(GameObject obj, Point from, Point to)
    {
        if (_animByObj.TryGetValue(obj, out var animator))
        {
            animator.MoveTo(GridToWorld(to));
        }
    }

    private void OnBoxStateChanged(Box box, BoxState oldState, BoxState newState)
    {
        if (_animByObj.TryGetValue(box, out var a) && a is BoxAnimator ba)
        {
            ba.State = newState; 
        }
    }

    private Vector2 GridToWorld(Point g)
        => _offset + new Vector2(g.X * TileSize, g.Y * TileSize);

    private bool TryGetDirectionJustPressed(out Direction dir)
    {
        dir = Direction.Down;
        var k = Core.Input.Keyboard;

        if (k.WasKeyJustPressed(Keys.Up) || k.WasKeyJustPressed(Keys.W)) { dir = Direction.Up; return true; }
        if (k.WasKeyJustPressed(Keys.Down) || k.WasKeyJustPressed(Keys.S)) { dir = Direction.Down; return true; }
        if (k.WasKeyJustPressed(Keys.Left) || k.WasKeyJustPressed(Keys.A)) { dir = Direction.Left; return true; }
        if (k.WasKeyJustPressed(Keys.Right) || k.WasKeyJustPressed(Keys.D)) { dir = Direction.Right; return true; }
        if (k.WasKeyJustPressed(Keys.Enter) || k.WasKeyJustPressed(Keys.Space) ) { Core.ChangeScene(new PauseScene(this)); }

        return false;
    }
}
