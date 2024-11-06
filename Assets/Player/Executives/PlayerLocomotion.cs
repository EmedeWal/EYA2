using UnityEngine;

namespace EmeWillem
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerLocomotion : MonoBehaviour, IMovingProvider
    {
        private Transform _transform;
        private float _delta;

        private PlayerAnimatorManager _animatorManager;
        private PlayerAttackHandler _attackHandler;
        private CapsuleCollider _collider;
        private Rigidbody _rigidbody;
        private bool _canMove = true;
        private bool _canRotate = true;

        [Header("ROTATION")]
        [SerializeField] private float _rotationSpeed = 15f;

        [Header("HORIZONTAL")]
        [SerializeField] private float _movementSpeed = 45f;

        [Header("VERTICAL")]
        [SerializeField] private float _stepHeight = 0.5f;
        [SerializeField] private float _stepSmooth = 0.5f;
        [SerializeField] private float _gravity = 15f;
        private LayerMask _ignoreLayers;
        private float _groundCheckRadius;
        private float _groundCheckOffset;
        private bool _grounded = true;

        private Vector3 _movementDirection = Vector3.zero;
        private float _movementAmount;
        private float _horizontal;
        private float _vertical;
        private bool _moving;

        public float MovementSpeed { private get; set; }
        public bool Moving => _moving;

        public void Init()
        {
            _transform = transform;

            _animatorManager = GetComponent<PlayerAnimatorManager>();
            _attackHandler = GetComponent<PlayerAttackHandler>();
            _collider = GetComponent<CapsuleCollider>();
            _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.useGravity = false;

            _groundCheckRadius = _collider.radius + 0.05f;
            _groundCheckOffset = _collider.radius;

            int controllerLayer = LayerMask.NameToLayer("Controller");
            int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");

            _ignoreLayers = ~(1 << controllerLayer | 1 << damageColliderLayer);
            gameObject.layer = controllerLayer;

            ListenToAttackEvents(true);
        }

        public void Cleanup()
        {
            ListenToAttackEvents(false);
        }

        public void FixedTick(float delta, Vector3 horizontalDirection, Vector3 verticalDirection, float horizontalInput, float verticalInput, Transform lockOnTarget)
        {
            _delta = delta;

            _grounded = IsGrounded();
            ManageVerticalPosition();

            if (_grounded)
            {
                _horizontal = horizontalInput;
                _vertical = verticalInput;

                Vector3 horizontal = _horizontal * horizontalDirection;
                Vector3 vertical = _vertical * verticalDirection;
                float movement = Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput);
                _movementDirection = (horizontal + vertical).normalized;
                _movementAmount = Mathf.Clamp01(movement);

                _canMove = !_animatorManager.GetBool("InAction");

                if (_horizontal != 0 || _vertical != 0 && _canMove)
                {
                    _moving = true;
                }
                else
                {
                    _moving = false;
                }

                if (_canMove)
                {
                    ManageHorizontalPosition();
                }
                else
                {
                    _rigidbody.velocity = Vector3.zero;
                }

                if (_canRotate)
                {
                    ManageRotation(lockOnTarget);
                }
            }

            HandleAnimations(lockOnTarget);
        }

        private void ManageVerticalPosition()
        {
            if (!_grounded)
            {
                _movementDirection.y -= _gravity * _delta;
                _animatorManager.CrossFade("Fall");
            }
            else
            {
                _movementDirection.y = 0;
            }

            Vector3 newVelocity = _rigidbody.velocity;
            newVelocity.y = _movementDirection.y;
            _rigidbody.velocity = newVelocity;
        }

        private void ManageHorizontalPosition()
        {
            if (_canMove)
            {
                HandleStepClimb();

                _movementDirection *= MovementSpeed * _delta * _movementSpeed;
                _rigidbody.velocity = new(_movementDirection.x, _rigidbody.velocity.y, _movementDirection.z);
            }
            else
            {
                _rigidbody.velocity = new(0, _rigidbody.velocity.y, 0);
            }
        }

        private void ManageRotation(Transform lockOnTarget)
        {
            Vector3 targetDirection = lockOnTarget ? lockOnTarget.position - _transform.position : _movementDirection;
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, tr, _delta * _movementAmount * _rotationSpeed);
            _rigidbody.MoveRotation(targetRotation);
        }

        private void HandleStepClimb()
        {
            Vector3 forwardRayOrigin = _transform.position + Vector3.up * 0.1f;

            if (Physics.Raycast(forwardRayOrigin, _movementDirection, out RaycastHit forwardHit, _collider.radius + 0.1f))
            {
                float stepHeight = forwardHit.point.y - _transform.position.y;
                if (stepHeight > 0 && stepHeight <= _stepHeight)
                {
                    Vector3 upwardRayOrigin = forwardHit.point + Vector3.up * 0.1f;
                    if (Physics.Raycast(upwardRayOrigin, Vector3.down, out RaycastHit topHit, _stepHeight))
                    {
                        Vector3 stepUpPosition = new(_transform.position.x, topHit.point.y, _transform.position.z);
                        _transform.position = Vector3.Lerp(_transform.position, stepUpPosition, _stepSmooth);
                        _movementDirection *= 1.5f;
                    }
                }
            }
        }

        private void HandleAnimations(bool locked)
        {
            _animatorManager.UpdateAnimatorValues(_delta, _horizontal, _vertical, _grounded, locked, Moving);
        }

        #region Attack Events
        private void ListenToAttackEvents(bool listen)
        {
            if (listen)
            {
                _attackHandler.AttackHalfway += PlayerLocomotion_AttackHalfway;
                _attackHandler.AttackEnded += PlayerLocomotion_AttackEnded;
            }
            else
            {
                _attackHandler.AttackHalfway -= PlayerLocomotion_AttackHalfway;
                _attackHandler.AttackEnded -= PlayerLocomotion_AttackEnded;
            }
        }

        private void PlayerLocomotion_AttackHalfway(BaseAttackData attackData)
        {
            _canRotate = false;
        }

        private void PlayerLocomotion_AttackEnded(BaseAttackData attackData)
        {
            _canRotate = true;
        }
        #endregion

        private bool IsGrounded()
        {
            Vector3 sphereOrigin = _transform.position + (Vector3.up * _groundCheckOffset);
            bool sphereCheck = Physics.CheckSphere(sphereOrigin, _groundCheckRadius, _ignoreLayers);

            if (!sphereCheck && _grounded)
            {
                if (Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hit, _groundCheckOffset + _stepHeight, _ignoreLayers))
                {
                    Vector3 targetPosition = new(_transform.position.x, hit.point.y, _transform.position.z);
                    _transform.position = Vector3.Lerp(_transform.position, targetPosition, _gravity * _delta);
                    return true;
                }
            }
            return sphereCheck;
        }
    }
}