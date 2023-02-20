using CezaryTomczak.Asteroids.View.Asteroid.States;
using ModestTree;
using Zenject;

namespace CezaryTomczak.Asteroids.View.Asteroid
{
    public enum AsteroidStates
    {
        Moving,
        Destroyed,
        Hit
    }
    
    public class AsteroidStateFactory
    {
        readonly SignalBus _signalBus;

        readonly AsteroidStateMoving.Factory _movingFactory;
        readonly AsteroidStateDestroyed.Factory _destroyedFactory;
        readonly AsteroidStateHit.Factory _hitFactory;

        public AsteroidStateFactory(
            SignalBus signalBus,
            AsteroidStateMoving.Factory movingFactory,
            AsteroidStateDestroyed.Factory destroyedFactory,
            AsteroidStateHit.Factory  hitFactory)
        {
            _signalBus = signalBus;
            _movingFactory = movingFactory;
            _destroyedFactory = destroyedFactory;
            _hitFactory = hitFactory;
        }

        public AsteroidState CreateState(AsteroidStates state)
        {
            switch (state)
            {
                case AsteroidStates.Moving:
                {
                    return _movingFactory.Create();
                }
                case AsteroidStates.Destroyed:
                {
                    return _destroyedFactory.Create(_signalBus);
                }
                case AsteroidStates.Hit:
                {
                    return _hitFactory.Create();
                }
            }

            throw Assert.CreateException();
        }
    }
}