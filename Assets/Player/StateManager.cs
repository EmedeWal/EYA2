using UnityEngine;

public class StateManager : MonoBehaviour
{
    Rigidbody _rigidbody;
    Animator _animator;

    [HideInInspector] public float Delta;
    public Vector3 MovementDirection;
    public float MovementAmount;
    public float Horizontal;
    public float Vertical;
    public float MovementSpeed = 2f;
    public float RotationSpeed = 5f;
    public bool Grounded;
    public bool LockedOn;
    public float GroundCheckOffset = 0.5f;
    public LayerMask LayersToIgnore;

    public LockOnTarget EnemyTarget;

    Transform _transform;
    int _animatorVertical;
    bool _canMove;

    public void Initialize()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        _rigidbody.angularDrag = 999;
        _rigidbody.drag = 4;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _animatorVertical = Animator.StringToHash("Vertical");

        int controllerLayer = LayerMask.NameToLayer("Controller");
        int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");

        gameObject.layer = controllerLayer;
        LayersToIgnore = ~(1 << controllerLayer | 1 << damageColliderLayer);

    }

    public void OnUpdate(float delta)
    {
        Delta = delta;

        Grounded = IsGrounded();
    }

    public void OnFixedUpdate(float delta)
    {
        Delta = delta;

        if (!_canMove) return;

        _rigidbody.drag = (MovementAmount > 0 || !Grounded) ? 0 : 4;

        if (Grounded)
        {
            _rigidbody.velocity = MovementDirection * (MovementSpeed * MovementAmount);
        }

        HandleRotation();
        HandleMovementAnimations();
    }

    void HandleMovementAnimations()
    {
        if (!Grounded) MovementAmount = 0;
        _canMove = _animator.GetBool("CanMove");
        _animator.SetFloat(_animatorVertical, MovementAmount, 0.1f, Delta);
    }

    void HandleRotation()
    {
        if (LockedOn) return;

        Vector3 targetDirection = MovementDirection;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, tr, Delta * MovementAmount * RotationSpeed);
        _transform.rotation = targetRotation;
    }

    public bool IsGrounded()
    {
        bool r = false;

        Vector3 origin = _transform.position + (Vector3.up * GroundCheckOffset);
        Vector3 direction = -Vector3.up;
        float distance = GroundCheckOffset + 0.25f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Vector3 targetPosition = hit.point;
            _transform.position = targetPosition;

            r = true;
        }

        return r;
    }

}
