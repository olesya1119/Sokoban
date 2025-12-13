using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameLibrary.Audio;

public class AudioController : IDisposable
{
    // Отслеживает созданные экземпляры звуковых эффектов, чтобы их можно было ставить на паузу,
    // снимать с паузы и/или освобождать (Dispose).
    private readonly List<SoundEffectInstance> _activeSoundEffectInstances;

    // Хранит громкость музыки (Song) для восстановления при выключении режима mute.
    private float _previousSongVolume;

    // Хранит громкость звуковых эффектов для восстановления при выключении режима mute.
    private float _previousSoundEffectVolume;

    /// <summary>
    /// Возвращает значение, указывающее, отключён ли звук (включён ли режим mute).
    /// </summary>
    public bool IsMuted { get; private set; }

    /// <summary>
    /// Получает или задаёт глобальную громкость музыки (Song).
    /// </summary>
    /// <remarks>
    /// Если IsMuted = true, геттер всегда возвращает 0.0f, а сеттер игнорирует установку громкости.
    /// </remarks>
    public float SongVolume
    {
        get
        {
            if (IsMuted)
            {
                return 0.0f;
            }

            return MediaPlayer.Volume;
        }
        set
        {
            if (IsMuted)
            {
                return;
            }

            MediaPlayer.Volume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Получает или задаёт глобальную громкость звуковых эффектов.
    /// </summary>
    /// <remarks>
    /// Если IsMuted = true, геттер всегда возвращает 0.0f, а сеттер игнорирует установку громкости.
    /// </remarks>
    public float SoundEffectVolume
    {
        get
        {
            if (IsMuted)
            {
                return 0.0f;
            }

            return SoundEffect.MasterVolume;
        }
        set
        {
            if (IsMuted)
            {
                return;
            }

            SoundEffect.MasterVolume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Возвращает значение, указывающее, был ли этот контроллер аудио освобождён (Dispose).
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Создаёт новый экземпляр аудио-контроллера.
    /// </summary>
    public AudioController()
    {
        _activeSoundEffectInstances = new List<SoundEffectInstance>();
    }

    // Финализатор, вызывается сборщиком мусора при очистке объекта.
    ~AudioController() => Dispose(false);

    /// <summary>
    /// Обновляет состояние этого аудио-контроллера.
    /// </summary>
    public void Update()
    {
        for (int i = _activeSoundEffectInstances.Count - 1; i >= 0; i--)
        {
            SoundEffectInstance instance = _activeSoundEffectInstances[i];

            if (instance.State == SoundState.Stopped)
            {
                if (!instance.IsDisposed)
                {
                    instance.Dispose();
                }
                _activeSoundEffectInstances.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Воспроизводит указанный звуковой эффект.
    /// </summary>
    /// <param name="soundEffect">Звуковой эффект, который нужно воспроизвести.</param>
    /// <returns>Экземпляр SoundEffectInstance, созданный этим методом.</returns>
    public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect)
    {
        return PlaySoundEffect(soundEffect, 1.0f, 0.0f, 0.0f, false);
    }

    /// <summary>
    /// Воспроизводит указанный звуковой эффект с заданными параметрами.
    /// </summary>
    /// <param name="soundEffect">Звуковой эффект, который нужно воспроизвести.</param>
    /// <param name="volume">Громкость в диапазоне от 0.0 (тишина) до 1.0 (максимум).</param>
    /// <param name="pitch">Изменение высоты тона: от -1.0 (на октаву ниже) до 0.0 (без изменений) до 1.0 (на октаву выше).</param>
    /// <param name="pan">Панорама: от -1.0 (левый канал) до 0.0 (по центру) до 1.0 (правый канал).</param>
    /// <param name="isLooped">Нужно ли зацикливать звуковой эффект после воспроизведения.</param>
    /// <returns>Экземпляр SoundEffectInstance, созданный при воспроизведении звукового эффекта.</returns>
    /// <returns>Экземпляр SoundEffectInstance, созданный этим методом.</returns>
    public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan, bool isLooped)
    {
        // Создаём экземпляр SoundEffectInstance из переданного SoundEffect.
        SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

        // Применяем указанные параметры громкости, pitch, pan и зацикливания.
        soundEffectInstance.Volume = volume;
        soundEffectInstance.Pitch = pitch;
        soundEffectInstance.Pan = pan;
        soundEffectInstance.IsLooped = isLooped;

        // Запускаем воспроизведение
        soundEffectInstance.Play();

        // Добавляем экземпляр в список активных для отслеживания
        _activeSoundEffectInstances.Add(soundEffectInstance);

        return soundEffectInstance;
    }

    /// <summary>
    /// Воспроизводит указанную композицию (Song).
    /// </summary>
    /// <param name="song">Композиция, которую нужно воспроизвести.</param>
    /// <param name="isRepeating">Нужно ли повторять композицию. По умолчанию — true.</param>
    public void PlaySong(Song song, bool isRepeating = true)
    {
        // Проверяем, играет ли уже MediaPlayer. Если да — останавливаем.
        // Если не остановить, на некоторых платформах это может вызвать проблемы.
        if (MediaPlayer.State == MediaState.Playing)
        {
            MediaPlayer.Stop();
        }

        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = isRepeating;
    }

    /// <summary>
    /// Ставит на паузу весь звук.
    /// </summary>
    public void PauseAudio()
    {
        // Ставим на паузу музыку (если играет).
        MediaPlayer.Pause();

        // Ставим на паузу все активные звуковые эффекты.
        foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Pause();
        }
    }

    /// <summary>
    /// Возобновляет воспроизведение всего ранее поставленного на паузу звука.
    /// </summary>
    public void ResumeAudio()
    {
        // Возобновляем музыку
        MediaPlayer.Resume();

        // Возобновляем все активные звуковые эффекты.
        foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Resume();
        }
    }

    /// <summary>
    /// Отключает звук (включает mute) для всего аудио.
    /// </summary>
    public void MuteAudio()
    {
        // Сохраняем уровни громкости, чтобы восстановить их при UnmuteAudio
        _previousSongVolume = MediaPlayer.Volume;
        _previousSoundEffectVolume = SoundEffect.MasterVolume;

        // Устанавливаем громкость в 0
        MediaPlayer.Volume = 0.0f;
        SoundEffect.MasterVolume = 0.0f;

        IsMuted = true;
    }

    /// <summary>
    /// Включает звук обратно, восстанавливая громкость до значений, которые были до отключения.
    /// </summary>
    public void UnmuteAudio()
    {
        // Восстанавливаем предыдущие значения громкости.
        MediaPlayer.Volume = _previousSongVolume;
        SoundEffect.MasterVolume = _previousSoundEffectVolume;

        IsMuted = false;
    }

    /// <summary>
    /// Переключает текущее состояние mute (вкл/выкл).
    /// </summary>
    public void ToggleMute()
    {
        if (IsMuted)
        {
            UnmuteAudio();
        }
        else
        {
            MuteAudio();
        }
    }

    /// <summary>
    /// Освобождает ресурсы этого аудио-контроллера и выполняет очистку.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Освобождает ресурсы этого аудио-контроллера и выполняет очистку.
    /// </summary>
    /// <param name="disposing">Указывает, нужно ли освобождать управляемые ресурсы.</param>
    protected void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
            {
                soundEffectInstance.Dispose();
            }
            _activeSoundEffectInstances.Clear();
        }

        IsDisposed = true;
    }
}
