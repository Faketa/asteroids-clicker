using UnityEngine;
using Zenject;

namespace CezaryTomczak.Asteroids.View.Asteroid.States
{
    public class AsteroidStateHit : AsteroidState
    {
        readonly Asteroid.Settings _settings;
        
        
        public AsteroidStateHit(Asteroid.Settings settings, ExplosionFactory explosionFactory)
        {
            _settings = settings;
        }
        
        public override void Update() { }

        public override void Start()
        {
            Renderer renderer = Asteroid.GetComponent<Renderer>();
            int colorIndex = Asteroid.CurrentHitPoints - 1;
            Color color = _settings.HitColors[colorIndex];
            renderer.material.SetColor("_Color", color);
        }

        public class Factory : PlaceholderFactory<AsteroidStateHit> { }
    }
}