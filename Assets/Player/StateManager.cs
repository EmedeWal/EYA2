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
    public bool IsGrounded;
    public float GroundCheckOffset = 0.5f;
    public LayerMask LayersToIgnore;

    int _animatorVertical;

    public void Initialize()
    {
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

        IsGrounded = Grounded();
    }

    public void OnFixedUpdate(float delta)
    {
        Delta = delta;

        _rigidbody.drag = (MovementAmount > 0) ? 0 : 4;
        _rigidbody.velocity = MovementDirection * (MovementSpeed * MovementAmount);

        HandleRotation();
        HandleMovementAnimations();
    }

    void HandleMovementAnimations()
    {
        _animator.SetFloat(_animatorVertical, MovementAmount, 0.1f, Delta);
    }

    void HandleRotation()
    {
        Vector3 targetDirection = MovementDirection;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Delta * MovementAmount * RotationSpeed);
        transform.rotation = targetRotation;
    }

    public bool Grounded()
    {
        bool r = false;

        RaycastHit hit;
        Vector3 origin = transform.position + (Vector3.up * GroundCheckOffset);
        Vector3 direction = -Vector3.up;
        float distance = GroundCheckOffset + 0.25f;

        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            r = true;

            Vector3 targetPosition = hit.point;
            transform.position = targetPosition;
        }

        return r;
    }

}