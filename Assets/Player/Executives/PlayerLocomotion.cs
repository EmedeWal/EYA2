using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    private Transform _transform;
    private float _delta;

    private PlayerAnimatorManager _animatorManager;
    private CharacterController _characterController;

    [Header("STAT REFERENCE")]
    [SerializeField] private PlayerStats _stats;

    [Header("ROTATION")]
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("GROUND CHECK")]
    [SerializeField] private float _gravity = 15f;
    private LayerMask _ignoreLayers;
    private float _groundCheckRadius;
    private float _groundCheckOffset;
    private float _grounding = 2f;
    private bool _grounded = true;

    private Vector3 _movementDirection = Vector3.zero;
    private float _movementAmount;
    private float _movementSpeed;
    private float _horizontal;
    private float _vertical;

    public float MovementSpeed { private get; set; }
    public bool IsMoving { get; private set; }

    public void Init()
    {
        _transform = transform;

        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _characterController = GetComponent<CharacterController>();

        _groundCheckRadius = _characterController.radius + 0.05f;
        _groundCheckOffset = _characterController.radius;

        int controllerLayer = LayerMask.NameToLayer("Controller");
        int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");

        _ignoreLayers = ~(1 << controllerLayer | 1 << damageColliderLayer);
        gameObject.layer = controllerLayer;
    }

    public void Tick(float delta, Vector3 horizontalDirection, Vector3 verticalDirection, float horizontalInput, float verticalInput, Transform lockOnTarget, bool lockedOn)
    {
        _delta = delta;

        _grounded = IsGrounded();
        ManageVerticalPosition();
        IsMoving = _characterController.velocity.magnitude > 0;

        if (_animatorManager.GetBool("InAction") || !_grounded) return;

        _horizontal = horizontalInput;
        _vertical = verticalInput;

        Vector3 horizontal = _horizontal * horizontalDirection;
        Vector3 vertical = _vertical * verticalDirection;
        float movement = Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput);
        _movementDirection = (horizontal + vertical).normalized;
        _movementAmount = Mathf.Clamp01(movement);
        _movementDirection *= MovementSpeed;
        _movementDirection.y = 0;

        ManageHorizontalPosition();
        HandleAnimations(lockedOn);
        ManageRotation(lockOnTarget, lockedOn);
    }

    private void ManageVerticalPosition()
    {
        if (_grounded)
        {
            _movementDirection.y -= _grounding;
        }
        else
        {
            _movementDirection.y -= _gravity * _delta;
            _animatorManager.CrossFadeAnimation(_delta, "Fall");
            _characterController.Move(_movementDirection * _delta);
        }
    }

    private void ManageHorizontalPosition()
    {
        _characterController.Move(_movementDirection * _delta);
    }

    private void ManageRotation(Transform lockOnTarget, bool lockedOn)
    {
        Vector3 targetDirection = lockedOn ? lockOnTarget.position - _transform.position : _movementDirection;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, tr, _delta * _movementAmount * _rotationSpeed);
        _transform.rotation = targetRotation;
    }

    private void HandleAnimations(bool lockedOn)
    {
        _animatorManager.UpdateAnimatorValues(_delta, _horizontal, _vertical, _grounded, lockedOn);
    }

    private bool IsGrounded()
    {
        Vector3 sphereOrigin = _transform.position + (Vector3.up * _groundCheckOffset);
        return Physics.CheckSphere(sphereOrigin, _groundCheckRadius, _ignoreLayers);
    }
}
