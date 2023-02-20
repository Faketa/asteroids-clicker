using Zenject;

namespace CezaryTomczak.Asteroids.View.Asteroid.States
{
    public class AsteroidStateMoving : AsteroidState
    {
        public override void Update() { }
        
        public class Factory : PlaceholderFactory<AsteroidStateMoving> { }
    }
}