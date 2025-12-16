using MonoGameLibrary;
using System;

namespace Sokoban.Scenes.UI;

class PauseScene : InterfaceScene
{
    private GameScene _gameScene;

    public PauseScene(GameScene gameScene) :base()
    {
        _gameScene = gameScene;

        AddButton("Restart", 100, 10, 110, 20, HandleRestartClicked);
        AddButton("Menu", 100, 10, 110, 100, HandleMenuClicked);
    }

    private void HandleRestartClicked(object sender, EventArgs e)
    {
        Core.ChangeScene(new GameScene(_gameScene.LevelNumber));
    }

    private void HandleMenuClicked(object sender, EventArgs e)
    {
        Core.ChangeScene(new TitleScene());
    }
}

