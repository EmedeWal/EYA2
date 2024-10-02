using System.Collections.Generic;
using UnityEngine;

public class MovementTracking : MonoBehaviour
{
    [Header("MOVEMENT TRACKING")]
    [SerializeField] private float _historicalPositionDuration = 1f;   
    [SerializeField] private float _historicalPositionInterval = 0.1f; 
    private Queue<Vector3> _historicalVelocities;                     
    private Vector3 _previousPosition;                               
    private int _maxQueueSize;                                        
    private float _lastPositionTime;

    private Transform _transform;

    public void Init()
    {
        _historicalVelocities = new Queue<Vector3>();
        _maxQueueSize = Mathf.CeilToInt(_historicalPositionDuration / _historicalPositionInterval);
        _previousPosition = transform.position;
        _lastPositionTime = Time.time;

        _transform = transform;
    }

    public void Tick(float delta)
    {
        Vector3 currentVelocity = (transform.position - _previousPosition) / delta;
        currentVelocity.y = 0;

        if (_lastPositionTime + _historicalPositionInterval <= Time.time)
        {
            if (_historicalVelocities.Count == _maxQueueSize)
            {
                _historicalVelocities.Dequeue();
            }

            _historicalVelocities.Enqueue(currentVelocity);
            _lastPositionTime = Time.time;
        }

        _previousPosition = transform.position;
    }

    public Vector3 AverageVelocity
    {
        get
        {
            if (_historicalVelocities.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 average = Vector3.zero;

            foreach (Vector3 velocity in _historicalVelocities)
            {
                average += velocity;
            }

            average.y = 0; 

            return average / _historicalVelocities.Count;
        }
    }
}
