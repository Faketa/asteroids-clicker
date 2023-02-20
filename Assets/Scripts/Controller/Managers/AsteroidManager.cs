using System;
using System.Collections.Generic;
using CezaryTomczak.Asteroids.Model;
using CezaryTomczak.Asteroids.Util;
using ModestTree;
using UnityEngine;
using Zenject;
using Asteroid = CezaryTomczak.Asteroids.View.Asteroid.Asteroid;

namespace CezaryTomczak.Asteroids.Controller.Managers
{
    public class AsteroidManager : ITickable, IFixedTickable
    {
        readonly List<Asteroid> _asteroids = new ();
        readonly SignalBus _signalBus;
        readonly Settings _settings;
        readonly Asteroid.Factory _asteroidFactory;
        readonly LevelHelper _level;

        bool _started;

        [InjectOptional]
        bool _autoSpawn = true;

        public AsteroidManager(
            SignalBus signalBus, Settings settings, Asteroid.Factory asteroidFactory, LevelHelper level)
        {
            _signalBus = signalBus;
            _settings = settings;
            _asteroidFactory = asteroidFactory;
            _level = level;
            
            _signalBus.Subscribe<AsteroidDestroyedSignal>(OnAsteroidDestroyed);
        }

        public void Start()
        {
            Assert.That(!_started);
            _started = true;

            ResetAll();

            for (int i = 0; i < _settings.StartingSpawns; i++)
                SpawnNext();
        }

        void ResetAll()
        {
            foreach (var asteroid in _asteroids)
                GameObject.Destroy(asteroid.gameObject);

            _asteroids.Clear();
        }

        void OnAsteroidDestroyed(AsteroidDestroyedSignal args)
        {
            _asteroids.Remove(args.Target);
            GameObject.Destroy(args.Target.gameObject);
        }

        public void Stop()
        {
            _started = false;
        }

        public void FixedTick()
        {
            for (int i = 0; i < _asteroids.Count; i++)
                _asteroids[i].FixedTick();
        }

        public void Tick()
        {
            for (int i = 0; i < _asteroids.Count; i++)
                _asteroids[i].Tick();

            if (_started && _autoSpawn)
                if (_asteroids.Count < _settings.StartingSpawns)
                    SpawnNext();
        }

        public void SpawnNext()
        {
            if (CanSpawn())
                return;
            
            var asteroid = _asteroidFactory.Create();
            asteroid.Position = GetRandomStartPosition(asteroid.Scale);

            _asteroids.Add(asteroid);
        }

        bool CanSpawn()
        {
            if (_asteroids.Count >= _settings.MaxSpawns)
                return true;
            return false;
        }
        
        Vector3 GetRandomStartPosition(float scale)
        {
            var x = UnityEngine.Random.Range(_level.Left, _level.Right);
            var y = UnityEngine.Random.Range(_level.Top, _level.Bottom);
            return new Vector3(x, y, 0);
        }

        [Serializable]
        public class Settings
        {
            public int StartingSpawns;
            public int MaxSpawns;
        }
    }
}