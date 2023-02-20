using System;
using UnityEngine;

namespace CezaryTomczak.Asteroids.View.Asteroid.States
{
    public abstract class AsteroidState : IDisposable
    {
        public Asteroid Asteroid;
        public abstract void Update();

        public virtual void Start() { }

        public virtual void Dispose() { }

        public virtual void OnTriggerEnter(Collider other) { }
    }
}