using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Locomotion : MonoBehaviour
    {
        private CharacterController _characterController;
        private AnimatorManager _animatorManager;
        private Transform _transform;

        private LayerMask _ignoreLayers;
        private float _groundCheckRadius;
        private float _groundCheckOffset;
//        private bool _grounded = true;

        private Vector3 _movementDirection;
        private Vector3 _currentVelocity;
        private float _movementAmount;

        [Header("SPEED SETTINGS")]
        [SerializeField] private float _rotationSpeed = 3f;
        [SerializeField] private float _acceleration = 3f;
        [SerializeField] private float _deceleration = 3f;
        [SerializeField] private float _dashSpeed = 3f;
        [SerializeField] private float _walkSpeed = 1.5f;
        private float _locomotion;
        private float _speed;

        public bool Dashing { get; private set; }

        public void Init()
        {
            _characterController = GetComponent<CharacterController>();
            _animatorManager = GetComponent<AnimatorManager>();
            _transform = transform;

            _characterController.center = new Vector3(0, 1, 0);
            _characterController.height = 1.8f;
            _characterController.radius = 0.2f;

            _groundCheckRadius = _characterController.radius + 0.05f;
            _groundCheckOffset = _characterController.radius;

            int controllerLayer = LayerMask.NameToLayer("Controller");
            int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");

            _ignoreLayers = ~(1 << controllerLayer | 1 << damageColliderLayer);
            gameObject.layer = controllerLayer;
        }

        public void Tick(float delta, Transform target, Vector3 xDirection, Vector3 yDirection, Vector2 input)
        {
            xDirection *= input.x;
            yDirection *= input.y;
            _movementAmount = input.magnitude;
            _speed = target ? _walkSpeed : _dashSpeed;
            Dashing = _locomotion > 0.9f && _speed == _dashSpeed;
            _movementDirection = (xDirection + yDirection).normalized;

            _animatorManager.UpdateAnimatorValues(delta, _locomotion, input.x, input.y, true, target);

            if (_animatorManager.GetBool("CanRotate"))
            {
                HandleRotation(delta, target);
            }

            SmoothAdjustVelocity(delta, _speed);
            HandleHorizontal(delta);
        }

        private void HandleHorizontal(float delta)
        {
            _characterController.Move(new Vector3(_currentVelocity.x, 0, _currentVelocity.z) * delta);
        }

        private void HandleRotation(float delta, Transform target)
        {
            Vector3 targetDirection = target ? target.position - _transform.position : _movementDirection;
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, lookRotation, delta * _movementAmount * _rotationSpeed);
            _transform.rotation = targetRotation;
        }

        private void SmoothAdjustVelocity(float delta, float speed)
        {
            if (_movementAmount < 0.1f || _animatorManager.GetBool("InAction")) 
            {
                _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, delta * _deceleration);
            }
            else 
            {
                _currentVelocity = Vector3.Slerp(_currentVelocity, _movementDirection * speed, delta * _acceleration);
            }

            _locomotion = _currentVelocity.magnitude / Mathf.Max(_dashSpeed, _walkSpeed);
        }
    }
}