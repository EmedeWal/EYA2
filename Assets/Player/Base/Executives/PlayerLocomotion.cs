using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerLocomotion : MonoBehaviour
{
    // References
    private PlayerAnimatorManager _animatorManager;
    private PlayerDataManager _dataManager;
    private Rigidbody _rigidbody;

    // Variables
    private Transform _transform;
    private float _delta;

    [Header("PLAYER STATS")]
    [SerializeField] private PlayerStats _stats;

    [Header("MOVEMENT")]
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

    public void Init()
    {
        _transform = transform;

        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.drag = _drag;
        _rigidbody.angularDrag = _angularDrag;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        int controllerLayer = LayerMask.NameToLayer("Controller");
        int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");

        gameObject.layer = controllerLayer;
        _layersToIgnore = ~(1 << controllerLayer | 1 << damageColliderLayer);
    }

    public void Tick()
    {
        _grounded = IsGrounded();
    }

    public void FixedTick(float delta, Vector3 horizontalDirection, Vector3 verticalDirection, float horizontalInput, float verticalInput, Transform lockOnTarget, bool lockedOn)
    {
        _delta = delta;

        _horizontal = horizontalInput;
        _vertical = verticalInput;

        Vector3 horizontal = _horizontal * horizontalDirection;
        Vector3 vertical = _vertical * verticalDirection;
        float movement = Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput);
        _movementDirection = (horizontal + vertical).normalized;
        _movementAmount = Mathf.Clamp01(movement);

        HandleAnimations(lockedOn);

        if (_animatorManager.GetBool("InAction"))
        {
            if (!_dataManager.LocomotionStruct.ForceAdded)
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
        else
        {
            _rigidbody.drag = (_movementAmount > 0 || !_grounded) ? 0 : _drag;

            if (_grounded)
            {
                _rigidbody.velocity = _movementDirection * (_movementSpeed * _movementAmount * _stats.GetStat(Stat.MovementSpeedModifier));
            }

            HandleRotation(lockOnTarget, lockedOn);
        }
    }

    public void AddForce(Vector3 direction)
    {
        _rigidbody.AddForce(direction, ForceMode.Force);
        _dataManager.LocomotionStruct.ForceAdded = true;
    }

    public void RemoveForce()
    {
        _dataManager.LocomotionStruct.ForceAdded = false;
    }

    private void HandleAnimations(bool lockedOn)
    {
        float horizontal;
        float vertical;

        if (lockedOn && _vertical >= 0)
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

    private void HandleRotation(Transform lockOnTarget, bool lockedOn)
    {
        Vector3 targetDirection = (lockedOn && _vertical >= 0) ? lockOnTarget.position - _transform.position : _movementDirection;
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
