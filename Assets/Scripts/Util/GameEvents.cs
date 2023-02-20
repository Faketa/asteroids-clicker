using CezaryTomczak.Asteroids.View.Asteroid;
using UnityEngine;

namespace CezaryTomczak.Asteroids.Util
{
    public class AsteroidHitSignal
    {
        public Collider TargetCollider;
    }
    
    public class AsteroidDestroyedSignal
    {
        public Asteroid Target;
    }
    
    public class SoundsStateSignal { }
    public class ScoreUpSignal { }
    public class ButtonClickSignal { }
}
