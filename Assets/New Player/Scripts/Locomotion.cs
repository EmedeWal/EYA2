using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    namespace Player
    {
        public class Locomotion : MonoBehaviour
        {
            public Vector3 MovementDirection { get; private set; }

            private AnimatorManager _animatorManager;
            private CapsuleCollider _capsuleCollider;
            private Rigidbody _rigidbody;
            private Transform _transform;
            private LayerMask _ignoreLayers;

            [Header("INPUT SETTINGS")]
            [SerializeField] private int _inputQueueSize = 3;
            [SerializeField] private float _inputDropThreshold = 0.3f;

            [Header("ANIMATION SETTINGS")]
            [SerializeField] private float _fastLocomotionThreshold = 0.7f;
            [SerializeField] private float _slowLocomotionThreshold = 0.3f;

            [Header("TURN SETTINGS")]
            [SerializeField] private float _fullTurnDotThreshold = -0.7f;
            [SerializeField] private float _halfTurnDotThreshold = -0.1f;
            

            private Queue<float> _inputHistory;
            private float _currentInput;

            private int _locomotionHash;
            private int _dodgeRollHash;
            private int _rightTurnHash;
            private int _leftTurnHash;
            private int _fullTurnHash;
            private int _inActionHash;
            private int _kickHash;
            private int _layer;
            private float _transitionTime = 0.3f;

            public void Init()
            {
                _animatorManager = GetComponentInChildren<AnimatorManager>();
                _capsuleCollider = GetComponent<CapsuleCollider>();
                _rigidbody = GetComponent<Rigidbody>();
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

                _locomotionHash = Animator.StringToHash("Locomotion");
                _dodgeRollHash = Animator.StringToHash("Dodge Roll");
                _rightTurnHash = Animator.StringToHash("Right Turn");
                _leftTurnHash = Animator.StringToHash("Left Turn");
                _fullTurnHash = Animator.StringToHash("Full Turn");
                _inActionHash = Animator.StringToHash("InAction");
                _kickHash = Animator.StringToHash("Kick");
                _layer = 0;

                _inputHistory = new Queue<float>(_inputQueueSize);

                GetComponent<InputHandler>().DodgeInputPerformed += Locomotion_DodgeInputPerformed;
            }

            public void FixedTick(float deltaTime, Vector3 xDirection, Vector3 yDirection, Vector2 rawInput)
            {
                xDirection *= rawInput.x;
                yDirection *= rawInput.y;
                MovementDirection = (xDirection + yDirection).normalized;

                _currentInput = rawInput.magnitude;
                if (_inputHistory.Count >= _inputQueueSize)
                {
                    _inputHistory.Dequeue();
                }
                _inputHistory.Enqueue(_currentInput);

                _animatorManager.UpdateAnimatorValues(deltaTime, _currentInput, 0.8f, true);

                if (_animatorManager.GetBool(_inActionHash) == false)
                {
                    CheckAnimations(yDirection);
                    HandleRotation(deltaTime);
                }
            }

            private void Locomotion_DodgeInputPerformed()
            {
                //Vector3 targetDirection = MovementDirection;
                //targetDirection.y = 0;

                //if (targetDirection == Vector3.zero) targetDirection = _transform.forward;
                //_transform.LookAt(targetDirection);
                _animatorManager.CrossFadeAction(_dodgeRollHash, _layer, _transitionTime);
            }

            private void HandleRotation(float delta)
            {
                Vector3 targetDirection = MovementDirection;
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

                float rotationSpeed = delta * _rigidbody.velocity.magnitude;
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, lookRotation, rotationSpeed);
                _transform.rotation = targetRotation;
            }

            private void CheckAnimations(Vector3 yDirection)
            {
                Vector3 movementDirection = MovementDirection;
                Vector3 forwardDirection = transform.forward;
                forwardDirection.y = 0;

                float initialInput = _inputHistory.Count > 0 ? _inputHistory.Peek() : _currentInput;
                float averageDrop = initialInput - _currentInput;

                float locomotion = _animatorManager.GetFloat(_locomotionHash);
                float dotProduct = Vector3.Dot(forwardDirection.normalized, movementDirection);

                if (dotProduct < _fullTurnDotThreshold && locomotion > _fastLocomotionThreshold)
                {
                    _animatorManager.CrossFadeAction(_fullTurnHash, _layer, _transitionTime);
                }
                else if (locomotion < _slowLocomotionThreshold)
                {
                    if (dotProduct < _halfTurnDotThreshold && dotProduct > _fullTurnDotThreshold)
                    {
                        HandleHalfTurns(movementDirection, forwardDirection, yDirection);
                    }
                    else if (averageDrop > _inputDropThreshold)
                    {
                        _animatorManager.CrossFadeAction(_kickHash, _layer, _transitionTime);
                    }
                }
            }

            private void HandleHalfTurns(Vector3 movementDirection, Vector3 forwardDirection, Vector3 yDirection)
            {
                Vector3 crossProduct = Vector3.Cross(yDirection, movementDirection);
                float cameraDot = Vector3.Dot(forwardDirection, yDirection);

                if (cameraDot >= 0)
                {
                    if (crossProduct.y > 0)
                    {
                        _animatorManager.CrossFadeAction(_rightTurnHash, _layer, _transitionTime);
                    }
                    else
                    {
                        _animatorManager.CrossFadeAction(_leftTurnHash, _layer, _transitionTime);
                    }
                }
                else
                {
                    if (crossProduct.y > 0)
                    {
                        _animatorManager.CrossFadeAction(_leftTurnHash, _layer, _transitionTime);
                    }
                    else
                    {
                        _animatorManager.CrossFadeAction(_rightTurnHash, _layer, _transitionTime);
                    }
                }
            }
        }
    }
}