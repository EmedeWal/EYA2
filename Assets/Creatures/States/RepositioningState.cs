using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RepositioningState : CreatureState
{
    private Transform _target;
    private Queue<Vector3> _waypoints;
    private Vector3 _currentWaypoint;
    private float _reachedDistanceThreshold = 1f;
    //private bool _rotateClockwise;

    public RepositioningState(CreatureAI creatureAI, Transform target) : base(creatureAI)
    {
        _target = target;
      //  _rotateClockwise = Random.value > 0.5f;
        _waypoints = GenerateCircularWaypoints();
    }

    public override void Enter()
    {
        _CreatureAI.Locomotion.StopAgent(false);
        MoveToNextWaypoint();
    }

    public override void Tick(float delta)
    {
        if (_waypoints.Count > 0)
        {
            if (Vector3.Distance(_CreatureAI.transform.position, _currentWaypoint) < _reachedDistanceThreshold)
            {
                MoveToNextWaypoint();
            }
        }
        else
        {
            if (Vector3.Distance(_CreatureAI.transform.position, _currentWaypoint) < _reachedDistanceThreshold)
            {
                _CreatureAI.SetState(new IdleState(_CreatureAI));
            }
        }
    }

    private Queue<Vector3> GenerateCircularWaypoints()
    {
        Queue<Vector3> waypoints = new();
        Vector3 startPos = _CreatureAI.Transform.position; 
        Vector3 directionTowardsPlayer = (_CreatureAI.DefaultTarget.position - startPos).normalized;
        Vector3 directionAwayFromPlayer = -directionTowardsPlayer;

        int numWaypoints = _CreatureAI.CreatureData.RetreatPoints;
        float currentRadius = _CreatureAI.CreatureData.RetreatRadius;
        float circleDegrees = _CreatureAI.CreatureData.RetreatDegrees;

        Vector3 leftDirection = Vector3.Cross(Vector3.up, directionAwayFromPlayer).normalized;
        float angleStep = circleDegrees / numWaypoints;

        for (int i = 0; i < numWaypoints; i++)
        {
            float angle = -(circleDegrees / 2) + (i * angleStep);
            Quaternion rotationOffset = Quaternion.Euler(0, 90, 0);
            Quaternion totalRotation = rotationOffset * Quaternion.Euler(0, angle, 0);
            Vector3 waypointDirection = totalRotation * directionAwayFromPlayer;
            Vector3 waypoint = startPos + waypointDirection * currentRadius + leftDirection * currentRadius;

            if (NavMesh.SamplePosition(waypoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                waypoints.Enqueue(hit.position);
            }
        }
        return waypoints;
    }

    private void MoveToNextWaypoint()
    {
        if (_waypoints.Count > 0)
        {
            _currentWaypoint = _waypoints.Dequeue();
            _CreatureAI.Locomotion.SetDestination(_currentWaypoint);
        }
    }
}