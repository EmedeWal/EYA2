using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References
    private PlayerStateManager _stateManager;
    private PlayerDataManager _dataManager;
    private CharacterController _controller;
    private Animator _animator;

    [Header("VARIABLES")]
    //[SerializeField] private float _movementSpeed = 12f;
    [SerializeField] private float _gravity = 18f;

    private Vector3 _movementDirection;

    [Header("MOVEMENT TRACKING")]
    [SerializeField] private float historicalPositionDuration = 1f;
    [SerializeField] private float historicalPositionInterval = 0.1f;
    private Queue<Vector3> historicalVelocities;
    private Vector3 previousPosition;
    private int maxQueueSize;
    private float lastPositionTime;

    private void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        maxQueueSize = Mathf.CeilToInt(1f / historicalPositionInterval * historicalPositionDuration);
        historicalVelocities = new Queue<Vector3>(maxQueueSize);

        previousPosition = transform.position;
    }

    private void Update()
    {
        GetDirection();
        DetermineBehavior();
        TrackVelocity();
    }

    private void GetDirection()
    {
        //Vector2 directionValue = _dataManager.GetDirection();
        //_movementDirection = new(directionValue.x, 0, directionValue.y);
    }

    private void DetermineBehavior()
    {
        if (_stateManager.IsAttacking())
        {
            _animator.SetFloat("Speed", 0);
        }
        else if (_stateManager.CanMove())
        {
            if (_movementDirection != Vector3.zero)
            {
                _stateManager.SetMoving();
                Movement();
            }
            else
            {
                _stateManager.SetIdle();
            }

            _animator.SetFloat("Speed", _movementDirection.magnitude);
        }

        Ground();
    }

    private void Movement()
    {
        //_controller.Move(_dataManager.GetMovementModifier() * _movementSpeed * Time.deltaTime * _movementDirection);
    }

    private void Ground()
    {
        _controller.Move(new Vector3(0, _controller.velocity.y - _gravity, 0) * Time.deltaTime);
    }

    private void TrackVelocity()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        currentVelocity.y = 0;

        if (lastPositionTime + historicalPositionInterval <= Time.time)
        {
            if (historicalVelocities.Count == maxQueueSize)
            {
                historicalVelocities.Dequeue();
            }

            historicalVelocities.Enqueue(currentVelocity);
            lastPositionTime = Time.time;
        }

        previousPosition = transform.position;
    }

    public Vector3 AverageVelocity
    {
        get
        {
            if (historicalVelocities.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 average = Vector3.zero;

            foreach (Vector3 velocity in historicalVelocities)
            {
                average += velocity;
            }

            average.y = 0;

            return average / historicalVelocities.Count;
        }
    }

}
