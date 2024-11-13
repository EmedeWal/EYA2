using System.Collections.Generic;
using EmeWillem.Utilities;
using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class Controller : MonoBehaviour
        {
            [Header("COMPONENTS")]
            [SerializeField] private List<DefenseCollider> _defenseColliderList;
            [SerializeField] private List<OffenseCollider> _offenseColliderList;

            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private AttackHandler _attackHandler;
            [SerializeField] private CameraController _cameraController;
            [SerializeField] private LockMarker _lockMarker;
            private Locomotion _locomotion;
            private Lock _lock;
            private Health _health;
            private Posture _posture;

            private void Awake()
            {
                Init();
            }

            private void FixedUpdate()
            {
                FixedTick(Time.fixedDeltaTime);
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
                _animatorManager = GetComponentInChildren<AnimatorManager>();
                _attackHandler = GetComponentInChildren<AttackHandler>();
                _locomotion = GetComponent<Locomotion>();
                _lock = GetComponent<Lock>();
                _health = GetComponent<Health>();
                _posture = GetComponent<Posture>();

                _inputHandler.Init();
                _cameraController.Init(transform);
                _locomotion.Init();
                _lock.Init();
                _health.Init(1000);
                _posture.Init(1000, 100);
                _attackHandler.Init(_offenseColliderList);
                _animatorManager.Init(1, 1.2f);

                foreach (DefenseCollider defenseCollider in _defenseColliderList)
                {
                    defenseCollider.Init(gameObject, _health, _posture, LayerMask.GetMask("Controller"));
                }

                foreach (OffenseCollider offenseCollider in _offenseColliderList)
                {
                    offenseCollider.Init(transform, LayerMask.GetMask("DamageCollider"));
                }
            }

            public void Cleanup()
            {
                _inputHandler.Cleanup();
                _locomotion.Cleanup();
                _lock.Cleanup();
                _attackHandler.Cleanup();
            }

            public void FixedTick(float delta)
            {
                Transform lockTarget = _lock.Target;
                Vector3 cameraForward = Directions.Normalize(_cameraController._CameraTransform.forward);
                Vector3 cameraRight = Directions.Normalize(_cameraController._CameraTransform.right);

                _locomotion.FixedTick(delta, lockTarget, cameraForward, cameraRight, _inputHandler.LeftStickInput);
                _cameraController.FixedTick(delta, lockTarget, _inputHandler.RightStickInput);
            }

            public void LateTick()
            {
                _health.LateTick();
                _posture.LateTick();
            }
        }
    }
}