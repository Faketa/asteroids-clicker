using System;
using CezaryTomczak.Asteroids.Controller.Managers;
using CezaryTomczak.Asteroids.Installers;
using CezaryTomczak.Asteroids.Util;
using ModestTree;
using UnityEngine;
using Zenject;

namespace CezaryTomczak.Asteroids.Controller.Handlers
{
    public class GUIHandler : MonoBehaviour, IDisposable, IInitializable
    {
        GameManager _gameManager;

        [SerializeField]
        GUIStyle _titleStyle;

        [SerializeField]
        GUIStyle _scoreStyle;

        [SerializeField]
        GUIStyle _buttonStyle;

        [SerializeField]
        GUIStyle _buttonSoundStyle;
        
        SignalBus _signalBus;
        GameInstaller.Settings _settings;

        [Inject]
        public void Construct(SignalBus signalBus, GameManager gameManager, GameInstaller.Settings settings)
        {
            _gameManager = gameManager;
            _signalBus = signalBus;
            _settings = settings;
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(50);
                    if (GUILayout.Button(_gameManager.SoundButtonText, _buttonSoundStyle))
                        _signalBus.Fire<SoundsStateSignal>(new SoundsStateSignal());
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                switch (_gameManager.State)
                {
                    case GameStates.WaitingToStart:
                    {
                        StartGui();
                        break;
                    }
                    case GameStates.Playing:
                    {
                        PlayingGui();
                        break;
                    }
                    case GameStates.GameCompleted:
                    {
                        WinGui();
                        break;
                    }
                    default:
                    {
                        Assert.That(false);
                        break;
                    }
                }
            }
            GUILayout.EndArea();
        }

        void PlayingGui()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(50);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(50);
                    GUILayout.Label(_settings.PointsText + _gameManager.Score.ToString(), _scoreStyle);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        void StartGui()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(100);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical();
                    {
                        GUILayout.FlexibleSpace();

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(_settings.GameTitleText, _titleStyle);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(60);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();

                            if (GUILayout.Button(_settings.StartText, _buttonStyle))
                                _signalBus.Fire<ButtonClickSignal>(new ButtonClickSignal());

                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }

                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        void WinGui()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(100);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical();
                    {
                        GUILayout.FlexibleSpace();

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(_settings.WinText, _titleStyle);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(60);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();

                            if (GUILayout.Button(_settings.RepeatText, _buttonStyle))
                                _signalBus.Fire<ButtonClickSignal>(new ButtonClickSignal());

                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }

                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
        
        public void Initialize() { }
        
        public void Dispose() { }
    }
}