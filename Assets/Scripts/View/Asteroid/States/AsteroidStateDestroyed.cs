using CezaryTomczak.Asteroids.Util;
using UnityEngine;
using Zenject;

namespace CezaryTomczak.Asteroids.View.Asteroid.States
{
    public class AsteroidStateDestroyed : AsteroidState
    {
        readonly SignalBus _signalBus;
        readonly ExplosionFactory _explosionFactory;
        
        GameObject _explosion;

        public AsteroidStateDestroyed(SignalBus signalBus, ExplosionFactory explosionFactory)
        {
            _signalBus = signalBus;
            _explosionFactory = explosionFactory;
        }
        public override void Start()
        {
            if (Asteroid.CurrentHitPoints == 0)
            {
                Asteroid.MeshRenderer.enabled = false;
                _explosion = _explosionFactory.Create().gameObject;
                _explosion.transform.position = Asteroid.Position;
            }
            
            _signalBus.Fire<AsteroidDestroyedSignal>(new AsteroidDestroyedSignal(){Target = Asteroid});
        }

        public override void Dispose()
        {
            //GameObject.Destroy(_explosion);
        }
        
        public override void Update() { }
        
        public class Factory : PlaceholderFactory<SignalBus, AsteroidStateDestroyed> { }
    }
}