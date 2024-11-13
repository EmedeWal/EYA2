using EmeWillem.Utilities;
using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class Locomotion : MonoBehaviour
        {
            public Vector3 MovementDirection => _movementDirection;

            // References
            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private CapsuleCollider _capsuleCollider;
            private Transform _lockTarget;
            private Rigidbody _rigidbody;
            private Transform _transform;
            private LayerMask _ignoreLayers;
            private Vector3 _movementDirection;
            private Vector3 _transformForward;
            private Vector3 _transformRight;
            private float _locomotion;

            [Header("TRANSITION SETTINGS")]
            [SerializeField] private float _locked = 0.4f;
            [SerializeField] private float _acceleration = 1f;
            [SerializeField] private float _deceleration = 0.2f;

            [Header("LOCOMOTION SETTINGS")]
            [SerializeField] private float _rotationSpeed = 3f;

            // Animations
            private int _dodgeStepBackwardHash;
            private int _dodgeStepForwardHash;
            private int _dodgeStepRightHash;
            private int _dodgeStepLeftHash;
            private int _locomotionHash;
            private int _dodgeRollHash;
            private int _kickHash;

            public void Init()
            {
                _inputHandler = GetComponent<InputHandler>();
                _animatorManager = GetComponentInChildren<AnimatorManager>();
                _capsuleCollider = GetComponent<CapsuleCollider>();
                _rigidbody = GetComponent<Rigidbody>();
                _movementDirection = transform.forward;
                _transform = transform;

                _rigidbody.interpolation = RigidbodyInterpolation.None;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                _rigidbody.isKinematic = false;
                _rigidbody.useGravity = false;

                _capsuleCollider.center = new Vector3(0, 0.95f, 0.1f);
                _capsuleCollider.height = 1.8f;
                _capsuleCollider.radius = 0.3f;

                int controllerLayer = LayerMask.NameToLayer("Controller");
                int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");
                _ignoreLayers = ~(1 << controllerLayer | 1 << damageColliderLayer);

                _dodgeStepBackwardHash = Animator.StringToHash("Dodge Step Backward");
                _dodgeStepForwardHash = Animator.StringToHash("Dodge Step Forward");
                _dodgeStepRightHash = Animator.StringToHash("Dodge Step Right");
                _dodgeStepLeftHash = Animator.StringToHash("Dodge Step Left");
                _locomotionHash = Animator.StringToHash("Locomotion");
                _dodgeRollHash = Animator.StringToHash("Dodge Roll");
                _kickHash = Animator.StringToHash("Kick");

                _inputHandler.DodgeInputPerformed += Locomotion_DodgeInputPerformed;
                // Subscribe to left stick press (kick input)
            }

            public void Cleanup()
            {
                _inputHandler.DodgeInputPerformed -= Locomotion_DodgeInputPerformed;
                // Unsubscribe to left stick press (kick input)
            }

            public void FixedTick(float deltaTime, Transform lockTarget, Vector3 cameraForward, Vector3 cameraRight, Vector2 rawInput)
            {
                UpdateVariables(lockTarget, cameraForward, cameraRight, rawInput);

                if (_animatorManager.Idle())
                {
                    float input = rawInput.magnitude;

                    float transitionTime = CalculateTransitionTime(input);
                    _animatorManager.UpdateAnimatorValues(deltaTime, input, rawInput.x, rawInput.y, transitionTime, true, lockTarget);

                    HandleRotation(deltaTime, input);
                }
                else if (_animatorManager.GetBool(Animator.StringToHash("InAction")))
                {
                    _animatorManager.UpdateAnimatorValues(deltaTime, 0, 0, 0, 0, true, lockTarget);
                }
            }

            private void UpdateVariables(Transform lockTarget, Vector3 cameraForward, Vector3 cameraRight, Vector2 rawInput)
            {
                Vector3 transformForward = Directions.Normalize(_transform.forward);
                Vector3 transformRight = Directions.Normalize(_transform.right);

                cameraRight *= rawInput.x;
                cameraForward *= rawInput.y;

                _lockTarget = lockTarget;
                _transformRight = transformRight;
                _transformForward = transformForward;
                _movementDirection = cameraRight + cameraForward;
                _locomotion = _animatorManager.GetFloat(_locomotionHash);

                if (_movementDirection == Vector3.zero) _movementDirection = _transformForward;
            }

            private void HandleRotation(float deltaTime, float input)
            {
                Vector3 targetDirection = _lockTarget ? _lockTarget.position - _transform.position : _movementDirection;
                targetDirection.y = 0;

                float rotationSpeed = deltaTime * input * _rotationSpeed;
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, lookRotation, rotationSpeed);
                _transform.rotation = targetRotation;
            }

            private void Locomotion_DodgeInputPerformed()
            {
                if (_lockTarget)
                {
                    // Direction calculation can be simplified because we are locking on now
                    DirectionType directionType = Directions.Translate(_movementDirection, _transformForward, _transformRight);

                    switch (directionType)
                    {
                        case DirectionType.None:
                            _animatorManager.CrossFade(_dodgeStepBackwardHash);
                            break;

                        case DirectionType.Backward:
                            _animatorManager.CrossFade(_dodgeStepBackwardHash);
                            break;

                        case DirectionType.Forward:
                            _animatorManager.CrossFade(_dodgeStepForwardHash);
                            break;

                        case DirectionType.Right:
                            _animatorManager.CrossFade(_dodgeStepRightHash);
                            break;

                        case DirectionType.Left:
                            _animatorManager.CrossFade(_dodgeStepLeftHash);
                            break;
                    }
                }
                else
                {
                    _animatorManager.CrossFade(_dodgeRollHash);
                }
            }

            private float CalculateTransitionTime(float input)
            {
                float deceleration = _deceleration * _locomotion;
                float transitionTime = _lockTarget ? _locked : _acceleration;
                return input > 0 ? transitionTime : deceleration;
            }
        }
    }
}