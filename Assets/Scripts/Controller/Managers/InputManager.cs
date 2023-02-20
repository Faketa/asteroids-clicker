using CezaryTomczak.Asteroids.Util;
using CezaryTomczak.Asteroids.View.Asteroid;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CezaryTomczak.Asteroids.Controller.Managers
{
    public class InputManager : MonoBehaviour
    {
        InputActions _inputActions;
        Camera _camera;
        SignalBus _signalBus;
        Asteroid.Settings _settings;
        
        [Inject]
        public void Construct(SignalBus signalBus, Asteroid.Settings settings, [Inject(Id = "Main")] Camera mainCamera)
        {
            _signalBus = signalBus;
            _settings = settings;
            _camera = mainCamera;
        }
        
        void Awake()
        {
            _inputActions = new();
        }

        void Start()
        {
            _inputActions.DefaultMap.Fire.performed += Fire;
        }

        void Fire(InputAction.CallbackContext callbackContext)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
                if (hitInfo.collider.CompareTag(_settings.TagName))
                    _signalBus.Fire(new AsteroidHitSignal() { TargetCollider = hitInfo.collider });
        }
        
        void OnEnable()
        {
            _inputActions.Enable();
        }

        void OnDisable()
        {
            _inputActions.Disable();
        }

        void OnDestroy()
        {
            _inputActions.DefaultMap.Fire.performed -= Fire;
        }
    }
}