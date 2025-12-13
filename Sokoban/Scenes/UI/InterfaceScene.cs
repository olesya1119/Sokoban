using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameGum;
using MonoGameLibrary.Scenes;
using System;
using System.Collections.Generic;
using MonoGameLibrary;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Scenes.UI;

/// <summary>
/// Базовый класс для создания сцен с применением Gum
/// </summary>
public class InterfaceScene : Scene
{

    protected const float DesignWidth = 1280f;
    protected const float DesignHeight = 720f;

    protected float ScaleX { get; private set; }
    protected float ScaleY { get; private set; }

    private List<Button> _buttons;
    private Panel _panel;

    private SoundEffect _buttonClockSoundEffect;

    public InterfaceScene()
    {
        _buttons = new List<Button>();
        var viewport = Core.GraphicsDevice.Viewport;

        ScaleX = viewport.Width / DesignWidth;
        ScaleY = viewport.Height / DesignHeight;
    }

    public override void Initialize()
    {
        base.Initialize();
        Core.ExitOnEscape = true;
        InitializeUI();
    }


    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));
        GumService.Default.Draw();
    }

    public override void Update(GameTime gameTime) => GumService.Default.Update(gameTime);

    private void InitializeUI()
    {
        GumService.Default.Root.Children.Clear();

        _panel = new Panel();
        _panel.Dock(Dock.Fill);
        _panel.AddToRoot();

        foreach (var button in _buttons)
            _panel.AddChild(button);

        // Фокус на первую кнопку
        if (_buttons.Count > 0)
            _buttons[0].IsFocused = true;
    }

  
    public override void LoadContent()
    {
        _buttonClockSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");
    }


    protected void AddButton(string text, float width, float height, float x, float y, EventHandler action)
    {
        var button = new Button();
        button.Text = text;
        button.Visual.Width = width * ScaleX;
        button.Visual.Height = height * ScaleY;
        button.Visual.X = x * ScaleX;
        button.Visual.Y = y * ScaleY;
        //button.Anchor(anchor);
        button.Click += HandleClicked;
        button.Click += action;

        _buttons.Add(button);
    }


    protected void HandleClicked(object sender, EventArgs e)
    {
        if (_buttonClockSoundEffect != null)
            Core.Audio.PlaySoundEffect(_buttonClockSoundEffect);
    }
}


