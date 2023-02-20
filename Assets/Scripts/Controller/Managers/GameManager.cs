using System;
using CezaryTomczak.Asteroids.Installers;
using CezaryTomczak.Asteroids.Util;
using ModestTree;
using UnityEngine;
using Zenject;

namespace CezaryTomczak.Asteroids.Controller.Managers
{
    public enum GameStates
    {
        WaitingToStart,
        Playing,
        GameCompleted
    }
    public class GameManager : IInitializable, ITickable, IDisposable
    {
        readonly GameInstaller.Settings _settings;
        readonly SignalBus _signalBus;
        readonly AsteroidManager _asteroidSpawner;

        GameStates _state = GameStates.WaitingToStart;
        int _score;
        string _soundButtonText;
        
        public GameManager(GameInstaller.Settings settings, AsteroidManager asteroidSpawner, SignalBus signalBus)
        {
            _settings = settings;
            _signalBus = signalBus;
            _asteroidSpawner = asteroidSpawner;

            _soundButtonText = SoundButtonText;
        }

        public GameStates State
        {
            get { return _state; }
        }
        
        public string SoundButtonText
        {
            get
            {
                string returnValue;
                returnValue = _settings.PlaySound ? _settings.SoundOffText : _settings.SoundOnText;
                return returnValue;
            }
        }
        
        public int Score
        {
            get { return _score; }
        }

        public void Initialize()
        {
            Physics.gravity = Vector3.zero;
            _signalBus.Subscribe<ScoreUpSignal>(ScoreUp);
            _signalBus.Subscribe<ButtonClickSignal>(StartGame);
            _signalBus.Subscribe<SoundsStateSignal>(OnSoundStateChanged);
        }

        void OnSoundStateChanged()
        {
            _settings.PlaySound = !_settings.PlaySound;
        }

        void ScoreUp()
        {
            _score++;
        }

        public void Tick()
        {
            switch (_state)
            {
                case GameStates.WaitingToStart:
                {
                    UpdateStarting();
                    break;
                }
                case GameStates.Playing:
                {
                    UpdatePlaying();
                    break;
                }
                case GameStates.GameCompleted:
                {
                    UpdateGameCompleted();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        void UpdateGameCompleted()
        {
            Assert.That(_state == GameStates.GameCompleted);
            _asteroidSpawner.Stop();
        }

        void UpdatePlaying()
        {
            Assert.That(_state == GameStates.Playing);
            if (_score >= _settings.WinScore)
                _state = GameStates.GameCompleted;
        }

        void UpdateStarting()
        {
            Assert.That(_state == GameStates.WaitingToStart);
        }

        void StartGame()
        {
            Assert.That(_state == GameStates.WaitingToStart || _state == GameStates.GameCompleted);
            _score = 0;
            _asteroidSpawner.Start();
            _state = GameStates.Playing;
        }
        
        public void Dispose() { }
    }
}