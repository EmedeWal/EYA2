using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class Controller : MonoBehaviour
        {
            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private AttackHandler _attackHandler;
            [SerializeField] private CameraController _cameraController;
            [SerializeField] private LockMarker _lockMarker;
            private Locomotion _locomotion;
            private Block _block;
            private Lock _lock;
            private Health _health;
            private Posture _posture;
            private DefenseCollider _defenseCollider;

            private void Awake()
            {
                Init();
            }

            private void Update()
            {
                Tick(Time.deltaTime);
            }

            private void LateUpdate()
            {
                LateTick();
            }

            private void OnDisable()
            {
                Cleanup();
            }

            public void Init()
            {
                _lockMarker.SingletonSetup();

                _inputHandler = GetComponent<InputHandler>();
                _animatorManager = GetComponent<AnimatorManager>();
                _attackHandler = GetComponent<AttackHandler>();
                _locomotion = GetComponent<Locomotion>();
                _block = GetComponent<Block>();
                _lock = GetComponent<Lock>();
                _health = GetComponent<Health>();
                _posture = GetComponent<Posture>();
                _defenseCollider = GetComponent<DefenseCollider>();

                _inputHandler.Init();
                _animatorManager.Init(1, 1.2f);
                _attackHandler.Init();
                _cameraController.Init(transform);
                _locomotion.Init();
                _block.Init();
                _lock.Init();
                _health.Init(1000);
                _posture.Init(new List<string> { "Stagger 1", "Stagger 2" }, 1000, 100);
                _defenseCollider.Init(gameObject, _health, _posture);
            }

            public void Cleanup()
            {
                _inputHandler.Cleanup();
                _attackHandler.Cleanup();
                _block.Cleanup();
                _lock.Cleanup();
            }

            public void Tick(float delta)
            {
                Transform lockTarget = _lock.Target;
                Vector3 xDirection = _cameraController._CameraTransform.right;
                Vector3 yDirection = _cameraController._CameraTransform.forward;

                _locomotion.Tick(delta, lockTarget, xDirection, yDirection, _inputHandler.LeftStickInput);
                _cameraController.Tick(delta, lockTarget, _inputHandler.RightStickInput);
            }

            public void LateTick()
            {
                _health.LateTick();
                _posture.LateTick();
            }
        }
    }
}