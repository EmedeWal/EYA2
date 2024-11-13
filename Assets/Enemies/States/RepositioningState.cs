//using System.Collections.Generic;
//using UnityEngine.AI;
//using UnityEngine;

//namespace EmeWillem
//{
//    public class RepositioningState : CreatureState
//    {
//        private Transform _followTarget;
//        private Queue<Vector3> _waypoints;
//        private Vector3 _currentWaypoint;
//        private int _retreatPoints;
//        private float _retreatRadius;
//        private float _retreatDegrees;
//        private float _distanceThreshold;
//        private bool _clockwiseRotation;

//        public RepositioningState(CreatureAI creatureAI, Transform target, int points, float radius, float degrees) : base(creatureAI)
//        {
//            _followTarget = target;
//            _retreatPoints = points;
//            _retreatRadius = radius;
//            _retreatDegrees = degrees;
//            _distanceThreshold = 1f;
//            _clockwiseRotation = Random.value < 0.5f;
//            _waypoints = GenerateCircularWaypoints();
//        }

//        public override void Enter()
//        {
//            _CreatureAI.ResetMovementFloats.StopAgent(false);
//            MoveToNextWaypoint();
//        }

//        public override void FixedTick(float delta)
//        {
//            if (_waypoints.Count > 0)
//            {
//                if (Vector3.Distance(_CreatureAI.transform.position, _currentWaypoint) < _distanceThreshold)
//                {
//                    MoveToNextWaypoint();
//                }
//            }
//            else
//            {
//                if (Vector3.Distance(_CreatureAI.transform.position, _currentWaypoint) < _distanceThreshold)
//                {
//                    _CreatureAI.SetState(new IdleState(_CreatureAI));
//                }
//            }
//        }

//        private Queue<Vector3> GenerateCircularWaypoints()
//        {
//            Queue<Vector3> waypoints = new();
//            Vector3 startPos = _CreatureAI.Transform.position;
//            Vector3 playerDirection = (_CreatureAI.DefaultTarget.position - startPos).normalized;
//            Vector3 lateralDirection = Vector3.Cross(Vector3.up, playerDirection).normalized;
//            float angleStep = _retreatDegrees / _retreatPoints;

//            if (_clockwiseRotation)
//            {
//                lateralDirection = -lateralDirection;
//                playerDirection = -playerDirection;
//            }

//            for (int i = 0; i < _retreatPoints; i++)
//            {
//                float angle = -(_retreatDegrees / 2) + (i * angleStep);

//                if (!_clockwiseRotation)
//                {
//                    angle = -angle;
//                }

//                Quaternion rotationOffset = Quaternion.Euler(0, 90, 0);
//                Quaternion totalRotation = rotationOffset * Quaternion.Euler(0, angle, 0);

//                Vector3 waypointDirection = totalRotation * playerDirection;
//                Vector3 waypoint = startPos + waypointDirection * _retreatRadius + lateralDirection * _retreatRadius;

//                if (NavMesh.SamplePosition(waypoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
//                {
//                    waypoints.Enqueue(hit.position);
//                }
//            }
//            return waypoints;
//        }

//        private void MoveToNextWaypoint()
//        {
//            if (_waypoints.Count > 0)
//            {
//                _currentWaypoint = _waypoints.Dequeue();
//                _CreatureAI.ResetMovementFloats.SetDestination(_currentWaypoint);
//            }
//        }
//    }
//}