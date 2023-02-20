using System;
using CezaryTomczak.Asteroids.Controller.Handlers;
using UnityEngine;
using Zenject;
using Asteroid = CezaryTomczak.Asteroids.View.Asteroid.Asteroid;
using AsteroidManager = CezaryTomczak.Asteroids.Controller.Managers.AsteroidManager;
using GameInstaller = CezaryTomczak.Asteroids.Installers.GameInstaller;

namespace CezaryTomczak.Asteroids.Controller.Installers
{
    [CreateAssetMenu(menuName = "Asteroids/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public AsteroidSettings Asteroid;
        public AudioHandler.Settings AudioHandler;
        public GameInstaller.Settings GameInstaller;

        [Serializable]
        public class AsteroidSettings
        {
            public AsteroidManager.Settings Spawner;
            public Asteroid.Settings General;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(Asteroid.Spawner);
            Container.BindInstance(Asteroid.General);
            Container.BindInstance(AudioHandler);
            Container.BindInstance(GameInstaller);
        }
    }
}