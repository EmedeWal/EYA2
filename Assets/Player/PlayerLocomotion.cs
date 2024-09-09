using System.Collections;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    // References
    private PlayerAnimatorManager _animatorManager;
    private Rigidbody _rigidbody;

    // Variables
    private Transform _transform;
    private float _delta;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    private Vector3 _movementDirection;
    private float _movementAmount;
    private float _horizontal;
    private float _vertical;
    private bool _canMove;

    // Rigidbody
    private float _drag = 4;
    private float _angularDrag = 999;

    // Grounded
    private LayerMask _layersToIgnore;
    private float _groundCheckOffset = 0.5f;
    private bool _grounded;

    // Other
    private bool _forceAdded;

    // DELETE LATER
    public LockOnTarget EnemyTarget;
    public bool LockedOn;

    public void Initialize()
    {
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;

        _rigidbody.drag = _drag;
        _rigidbody.angularDrag = _angularDrag;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        int controllerLayer = LayerMask.NameToLayer("Controller");
        int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");

        gameObject.layer = controllerLayer;
        _layersToIgnore = ~(1 << controllerLayer | 1 << damageColliderLayer);

    }

    public void OnUpdate()
    {
        _grounded = IsGrounded();
    }

    public void OnFixedUpdate(float delta, Vector3 horizontalDirection, Vector3 verticalDirection, float horizontalInput, float verticalInput)
    {
        _delta = delta;

        _horizontal = horizontalInput;
        _vertical = verticalInput;

        Vector3 horizontal = _horizontal * horizontalDirection;
        Vector3 vertical = _vertical * verticalDirection;
        float movement = Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput);
        _movementDirection = (horizontal + vertical).normalized;
        _movementAmount = Mathf.Clamp01(movement);

        HandleAnimations();

        if (_animatorManager.GetBool("InAction"))
        {
            if (!_forceAdded)
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
        else
        {
            _rigidbody.drag = (_movementAmount > 0 || !_grounded) ? 0 : _drag;

            if (_grounded)
            {
                _rigidbody.velocity = _movementDirection * (_movementSpeed * _movementAmount);
            }

            HandleRotation();
        }
    }

    public void AddForce(Vector3 direction)
    {
        _rigidbody.AddForce(direction, ForceMode.Force);
        _forceAdded = true;
    }

    public void RemoveForce()
    {
        _forceAdded = false;
    }

    private void HandleAnimations()
    {
        float horizontal;
        float vertical;
        bool lockedOn = LockedOn;

        if (LockedOn && _vertical >= 0)
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(_movementDirection);
            horizontal = relativeDirection.x;
            vertical = relativeDirection.z;
        }
        else
        { 
            horizontal = _movementAmount;
            vertical = _movementAmount;
            lockedOn = false;
        }

        _animatorManager.UpdateAnimatorValues(_delta, horizontal, vertical, _grounded, lockedOn);
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = (LockedOn && _vertical >= 0) ? EnemyTarget.transform.position - _transform.position : _movementDirection;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, tr, _delta * _movementAmount * _rotationSpeed);
        _transform.rotation = targetRotation;
    }

    private bool IsGrounded()
    {
        bool grounded = false;

        Vector3 origin = _transform.position + (Vector3.up * _groundCheckOffset);
        Vector3 direction = -Vector3.up;
        float distance = _groundCheckOffset + 0.25f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Vector3 targetPosition = hit.point;
            _transform.position = targetPosition;

            grounded = true;
        }

        return grounded;
    }
}
