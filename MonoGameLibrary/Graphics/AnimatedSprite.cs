using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class AnimatedSprite : Sprite
{
    private int _currentFrame;
    private TimeSpan _elapsed;
    private Animation _animation;

    /// <summary>
    /// Получает или задаёт анимацию для этого анимированного спрайта.
    /// </summary>
    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            Region = _animation.Frames[0];
        }
    }

    /// <summary>
    /// Создаёт новый анимированный спрайт.
    /// </summary>
    public AnimatedSprite() { }

    /// <summary>
    /// Создаёт новый анимированный спрайт с указанной анимацией.
    /// </summary>
    /// <param name="animation">Анимация для этого анимированного спрайта.</param>
    public AnimatedSprite(Animation animation)
    {
        Animation = animation;
    }

    /// <summary>
    /// Обновляет состояние этого анимированного спрайта.
    /// </summary>
    /// <param name="gameTime">Снимок временных параметров игры, предоставляемый фреймворком.</param>
    public void Update(GameTime gameTime)
    {
        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            _currentFrame++;

            if (_currentFrame >= _animation.Frames.Count)
            {
                _currentFrame = 0;
            }

            Region = _animation.Frames[_currentFrame];
        }
    }
}
