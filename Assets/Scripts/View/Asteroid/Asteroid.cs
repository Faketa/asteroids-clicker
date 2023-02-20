using System;
using System.Collections;
using System.Collections.Generic;
using CezaryTomczak.Asteroids.Model;
using CezaryTomczak.Asteroids.Util;
using CezaryTomczak.Asteroids.View.Asteroid.States;
using ModestTree;
using UnityEngine;
using Zenject;

namespace CezaryTomczak.Asteroids.View.Asteroid
{
    public class Asteroid : MonoBehaviour
    {
        LevelHelper _level;
        Rigidbody _rigidBody;
        Collider _collider;
        Settings _settings;
        SignalBus _signalBus;

        AsteroidAttributes _asteroidAttributes;
        
        AsteroidStateFactory _stateFactory;
        AsteroidState _state;
        
        bool _lifetimeCompleted;
        IEnumerator _lifetimeCoroutine;

        int _currentHitpoints;
        
        [SerializeField]
        MeshRenderer _meshRenderer;

        [Inject]
        public void Construct(AsteroidStateFactory stateFactory, SignalBus signalBus, LevelHelper level, Settings settings)
        {
            _stateFactory = stateFactory;
            _signalBus = signalBus;
            _level = level;
            _settings = settings;
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        void Start()
        {
            _signalBus.Subscribe<AsteroidHitSignal>(OnHit);
            
            GenerateRandomAttributes();

            _currentHitpoints = _settings.HitPoints;
            
            Scale = Mathf.Lerp(_settings.MinScale, _settings.MaxScale, _asteroidAttributes.SizePx);
            Mass = Mathf.Lerp(_settings.MinMass, _settings.MaxMass, _asteroidAttributes.SizePx);
            Velocity = GetRandomDirection() * _asteroidAttributes.InitialSpeed;

            _lifetimeCoroutine = WaitForLifetimeCompleted(_asteroidAttributes.Lifetime);
            StartCoroutine(_lifetimeCoroutine);
        }

        void OnHit(AsteroidHitSignal args)
        {
            if (_lifetimeCompleted)
                return;
            
            if (_collider == args.TargetCollider)
            {
                _currentHitpoints--;
                
                if (_currentHitpoints == 0)
                {
                    _state = _stateFactory.CreateState(AsteroidStates.Destroyed);
                    _state.Asteroid = this;
                    _state.Start();
                    
                    _signalBus.Fire<ScoreUpSignal>(new ScoreUpSignal());
                    
                    return;
                }
                
                _state = _stateFactory.CreateState(AsteroidStates.Hit);
                _state.Asteroid = this;
                _state.Start();
            }
        }

        void GenerateRandomAttributes()
        {
            var sizePx = UnityEngine.Random.Range(0.0f, 1.0f);
            var speed = UnityEngine.Random.Range(_settings.MinSpeed, _settings.MaxSpeed);
            var lifetime = UnityEngine.Random.Range(_settings.MinLifetimeSeconds, _settings.MaxLifetimeSeconds);
            
            _asteroidAttributes = new AsteroidAttributes(sizePx, speed, lifetime);
        }
        
        Vector3 GetRandomDirection()
        {
            var theta = UnityEngine.Random.Range(0, Mathf.PI * 2.0f);
            return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
        }

        public MeshRenderer MeshRenderer
        {
            get { return _meshRenderer; }
        }
        
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public float Mass
        {
            get { return _rigidBody.mass; }
            set { _rigidBody.mass = value; }
        }
        
        public int CurrentHitPoints
        {
            get { return _currentHitpoints; }
        }

        public float Scale
        {
            get
            {
                var scale = transform.localScale;
                Assert.That(scale[0] == scale[1] && scale[1] == scale[2]);

                return scale[0];
            }
            set
            {
                transform.localScale = new Vector3(value, value, value);
                _rigidBody.mass = value;
            }
        }

        public Vector3 Velocity
        {
            get { return _rigidBody.velocity; }
            set { _rigidBody.velocity = value; }
        }

        public void FixedTick()
        {
            var speed = _rigidBody.velocity.magnitude;

            if (speed > _settings.MaxSpeed)
            {
                var dir = _rigidBody.velocity / speed;
                _rigidBody.velocity = dir * _settings.MaxSpeed;
            }
        }

        public void Tick()
        {
            if (_lifetimeCompleted)
            {
                _state = _stateFactory.CreateState(AsteroidStates.Destroyed);
                _state.Asteroid = this;
                _state.Start();
            }
            CheckForTeleport();
        }

        void CheckForTeleport()
        {
            if (Position.x > _level.Right + Scale && IsMovingInDirection(Vector3.right))
                transform.SetX(_level.Left - Scale);
            else if (Position.x < _level.Left - Scale && IsMovingInDirection(-Vector3.right))
                transform.SetX(_level.Right + Scale);
            else if (Position.y < _level.Bottom - Scale && IsMovingInDirection(-Vector3.up))
                transform.SetY(_level.Top + Scale);
            else if (Position.y > _level.Top + Scale && IsMovingInDirection(Vector3.up))
                transform.SetY(_level.Bottom - Scale);

            transform.RotateAround(transform.position, Vector3.up, 30 * Time.deltaTime);
        }

        bool IsMovingInDirection(Vector3 dir)
        {
            return Vector3.Dot(dir, _rigidBody.velocity) > 0;
        }
        
        IEnumerator WaitForLifetimeCompleted(float lifetime)
        {
            while (true)
            {
                yield return new WaitForSeconds(lifetime);
                _lifetimeCompleted = true;
            }
        }
        
        [Serializable]
        public class Settings
        {
            public int MinLifetimeSeconds;
            public int MaxLifetimeSeconds;
            public int HitPoints;
            public float MinScale;
            public float MaxScale;
            public float MinMass;
            public float MaxMass;
            public float MinSpeed;
            public float MaxSpeed;
            public string TagName;
            public List<Color> HitColors;
        }

        readonly struct AsteroidAttributes
        {
            public AsteroidAttributes(float sizePx, float initialSpeed, float lifetime)
            {
                SizePx = sizePx;
                InitialSpeed = initialSpeed;
                Lifetime = lifetime;
            }
            
            public float SizePx { get; }
            public float InitialSpeed { get; }
            public float Lifetime { get; }
        }

        public class Factory : PlaceholderFactory<Asteroid> { }
    }
}