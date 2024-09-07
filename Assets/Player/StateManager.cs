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
    public float MovementSpeed = 2;

    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        _rigidbody.angularDrag = 999;
        _rigidbody.drag = 4;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void OnFixedUpdate(float delta)
    {
        Delta = delta;

        _rigidbody.drag = (MovementAmount > 0) ? 0 : 4;

        _rigidbody.velocity = MovementDirection * (MovementSpeed * MovementAmount);
    }
}
