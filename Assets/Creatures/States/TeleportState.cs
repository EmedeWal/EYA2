namespace EmeWillem
{
    using UnityEngine;
    using UnityEngine.AI;

    public class TeleportState : CreatureState
    {
        private Transform _target;
        private int _maxAttempts;
        private float _teleportRadius;
        private float _navMeshSampleDistance;

        public TeleportState(CreatureAI creature, Transform target, float teleportRadius) : base(creature)
        {
            _target = target;
            _maxAttempts = 30;
            _teleportRadius = teleportRadius;
            _navMeshSampleDistance = 2f;
        }

        public override void Enter()
        {
            PerformTeleport();
        }

        private void PerformTeleport()
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                Vector3 randomDirection = Random.onUnitSphere;
                randomDirection.y = 0;

                Vector3 targetPosition = _CreatureAI.transform.position + randomDirection * _teleportRadius;

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navHit, _navMeshSampleDistance, NavMesh.AllAreas))
                {
                    _CreatureAI.transform.position = navHit.position;

                    Vector3 directionToTarget = (_target.position - _CreatureAI.transform.position).normalized;
                    if (directionToTarget != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
                        _CreatureAI.transform.rotation = targetRotation;
                    }

                    break;
                }
            }
        }
    }
}