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

            [Header("ROTATION SETTINGS")]
            [SerializeField] private float _rotationSpeed = 4.5f;

            [Header("INPUT SETTINGS")]
            [SerializeField] private int _inputQueueSize = 3;
            [SerializeField] private float _inputDropThreshold = 0.3f;

            [Header("ANIMATION SETTINGS")]
            [SerializeField] private float _mediumLocomotionThreshold = 0.5f;
            [SerializeField] private float _slowLocomotionThreshold = 0.3f;

            [Header("TURN SETTINGS")]
            [SerializeField] private float _fullTurnDotThreshold = 0.5f;
            [SerializeField] private float _turnInputThreshold = 0.3f;
            [SerializeField] private float _turnTransitionTime = 0.3f;

            private Queue<float> _inputHistory;
            private float _currentInput;

            private int _fullTurnBlendHash;
            private int _locomotionHash;
            private int _rightTurnHash;
            private int _leftTurnHash;
            private int _inActionHash;
            private int _kickHash;
            private int _layer;

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

                _fullTurnBlendHash = Animator.StringToHash("Full Turn Blend");
                _locomotionHash = Animator.StringToHash("Locomotion");
                _rightTurnHash = Animator.StringToHash("Right Turn");
                _leftTurnHash = Animator.StringToHash("Left Turn");
                _inActionHash = Animator.StringToHash("InAction");
                _kickHash = Animator.StringToHash("Kick");
                _layer = 0;

                _inputHistory = new Queue<float>(_inputQueueSize);
            }

            public void FixedTick(float deltaTime, Vector3 xDirection, Vector3 yDirection, Vector2 rawInput)
            {
                if (_animatorManager.GetBool(_inActionHash) == false)
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

                    _animatorManager.UpdateAnimatorValues(deltaTime, _currentInput, true);
                    HandleRotation(deltaTime);
                    CheckAnimations(); 
                }
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

            private void CheckAnimations()
            {
                Vector3 inputDirection = MovementDirection;
                Vector3 currentForward = transform.forward;
                currentForward.y = 0;

                float initialInput = _inputHistory.Count > 0 ? _inputHistory.Peek() : _currentInput;
                float averageDrop = initialInput - _currentInput;

                float locomotion = _animatorManager.GetFloat(_locomotionHash);
                float dotProduct = Vector3.Dot(currentForward.normalized, inputDirection);

                if (dotProduct < -_fullTurnDotThreshold && locomotion > _mediumLocomotionThreshold)
                {
                    _animatorManager.CrossFadeAction(_fullTurnBlendHash, _layer);
                }
                else if (locomotion < _slowLocomotionThreshold && averageDrop > _inputDropThreshold)
                {
                    _animatorManager.CrossFadeAction(_kickHash, _layer);

                    //if (inputDirection.x > _turnInputThreshold)
                    //{
                    //    _animatorManager.CrossFadeAction(_leftTurnHash, _layer, _turnTransitionTime);
                    //}
                    //else if (inputDirection.x < -_turnInputThreshold)
                    //{
                    //    _animatorManager.CrossFadeAction(_rightTurnHash, _layer, _turnTransitionTime);
                    //}
                    //else
                    //{
                    //    _animatorManager.CrossFadeAction(_kickHash, _layer);
                    //}
                }
            }
        }
    }
}