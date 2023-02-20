using System;
using CezaryTomczak.Asteroids.Installers;
using CezaryTomczak.Asteroids.Util;
using UnityEngine;
using Zenject;

namespace CezaryTomczak.Asteroids.Controller.Handlers
{
    public class AudioHandler : IInitializable, IDisposable
    {
        readonly SignalBus _signalBus;
        readonly Settings _settings;
        readonly GameInstaller.Settings _mainSettings;
        readonly AudioSource _audioSource;

        public AudioHandler(
            AudioSource audioSource,
            Settings settings,
            SignalBus signalBus,
            GameInstaller.Settings mainSettings)
        {
            _signalBus = signalBus;
            _settings = settings;
            _mainSettings = mainSettings;
            _audioSource = audioSource;
            
            OnSoundStateChanged();
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ScoreUpSignal>(OnScoreUp);
            _signalBus.Subscribe<AsteroidHitSignal>(OnHit);
            _signalBus.Subscribe<SoundsStateSignal>(OnSoundStateChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ScoreUpSignal>(OnScoreUp);
            _signalBus.Unsubscribe<AsteroidHitSignal>(OnHit);
            _signalBus.Unsubscribe<SoundsStateSignal>(OnSoundStateChanged);
        }

        void OnScoreUp()
        {
            _audioSource.PlayOneShot(_settings.ExplosionSound);
        }
        
        void OnHit()
        {
            _audioSource.PlayOneShot(_settings.HitSound);
        }
        
        void OnSoundStateChanged()
        {
            AudioListener.pause = !_mainSettings.PlaySound;
        }

        [Serializable]
        public class Settings
        {
            public AudioClip ExplosionSound;
            public AudioClip HitSound;
        }
    }
}