using System;
using CezaryTomczak.Asteroids.Controller.Handlers;
using CezaryTomczak.Asteroids.Controller.Managers;
using CezaryTomczak.Asteroids.Model;
using CezaryTomczak.Asteroids.Util;
using CezaryTomczak.Asteroids.View.Asteroid;
using CezaryTomczak.Asteroids.View.Asteroid.States;
using UnityEngine;
using Zenject;

namespace CezaryTomczak.Asteroids.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        Settings _settings = null;
        public override void InstallBindings()
        {
            InstallAsteroids();
            InstallMisc();
            InstallSignals();
            InstallExecutionOrder();
        }
        
        void InstallAsteroids()
        {
            Container.Bind<AsteroidStateFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<AsteroidManager>().AsSingle();
            Container.BindFactory<Asteroid, Asteroid.Factory>()
                .FromComponentInNewPrefab(_settings.AsteroidPrefab)
                .WithGameObjectName("Asteroid")
                .UnderTransformGroup("Asteroids");

            Container.BindFactory<AsteroidStateMoving, AsteroidStateMoving.Factory>().WhenInjectedInto<AsteroidStateFactory>();
            Container.BindFactory<SignalBus, AsteroidStateDestroyed, AsteroidStateDestroyed.Factory>().WhenInjectedInto<AsteroidStateFactory>();
            Container.BindFactory<AsteroidStateHit, AsteroidStateHit.Factory>().WhenInjectedInto<AsteroidStateFactory>();
        }
        
        void InstallMisc()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.Bind<LevelHelper>().AsSingle();
            Container.Bind<InputManager>().AsSingle();
            
            Container.BindFactory<Transform, ExplosionFactory>()
                .FromComponentInNewPrefab(_settings.ExplosionPrefab);
            
            Container.BindInterfacesTo<AudioHandler>().AsSingle();
        }

        void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<AsteroidHitSignal>();
            Container.DeclareSignal<AsteroidDestroyedSignal>();
            Container.DeclareSignal<ScoreUpSignal>();
            Container.DeclareSignal<ButtonClickSignal>();
            Container.DeclareSignal<SoundsStateSignal>();
        }
        
        void InstallExecutionOrder()
        {
            Container.BindExecutionOrder<AsteroidManager>(-20);
            Container.BindExecutionOrder<GameManager>(-10);
        }
        
        [Serializable]
        public class Settings
        {
            public string GameTitleText;
            public string PointsText;
            public string WinText;
            public string RepeatText;
            public string StartText;
            public string SoundOnText;
            public string SoundOffText;
            public bool PlaySound;
            public int WinScore;
            public GameObject AsteroidPrefab;
            public GameObject ExplosionPrefab;
        }
    }
}