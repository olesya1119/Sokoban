using System;
using MonoGameLibrary;

namespace Sokoban.Scenes.UI;

public class TitleScene : InterfaceScene
{
    public TitleScene() : base()
    {
        AddButton("Start", 100, -10, 110, 40, HandleStartClicked);
        AddButton("Exit", 100, -10, 110, 100, HandleExitClicked);
    }

    private void HandleStartClicked(object sender, EventArgs e)
    {
        Core.ChangeScene(new LevelMenuScene());
    }

    private void HandleExitClicked(object sender, EventArgs e)
    {
        Core.Instance.Exit();
    }
}
