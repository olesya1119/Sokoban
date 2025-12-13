using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Gum.Forms.Controls;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace Sokoban.Scenes.UI;

public class TitleScene : InterfaceScene
{
    public TitleScene() : base()
    {
        AddButton("Start", 100, -10, 110, 40, HandleStartClicked);
        AddButton("Start1", 100, -10, 110, 70,  HandleStartClicked);
        AddButton("Start2", 100, -10, 110, 100,  HandleStartClicked);
    }

    private void HandleStartClicked(object sender, EventArgs e)
    {

    }

    private void HandleOptionsClicked(object sender, EventArgs e)
    {
    }

    private void HandleExitClicked(object sender, EventArgs e)
    {

        // Корректно закрываем игру
        Core.Instance.Exit();
    }
}
