using UnityEngine;
using UnityEngine.AI;

namespace EmeWillem.AI
{
    public class Locomotion : MonoBehaviour
    {
        private AnimatorManager _animatorManager;
        private NavMeshAgent _navMeshAgent;

        private float _walkSpeed;
        private float _runSpeed;

        public void Init(float walkSpeed, float runSpeed)
        {
            _animatorManager = GetComponent<AnimatorManager>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = runSpeed;

            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
        }

        public void SetDestination(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
        }

        public void StopAgent(bool stop)
        {
            _navMeshAgent.isStopped = stop;
        }

        public void UpdateRotation(bool update)
        {
            _navMeshAgent.updateRotation = update;
        }

        public void SetSpeed(float speed)
        {
            _navMeshAgent.speed = speed * _animatorManager.MovementSpeed;
        }

        public float CalculateLocomotionValue()
        {
            float currentSpeed = _navMeshAgent.velocity.magnitude;

            if (currentSpeed <= _walkSpeed)
            {
                return Mathf.Lerp(0, 0.5f, currentSpeed / _walkSpeed);
            }
            else if (currentSpeed <= _runSpeed)
            {
                return Mathf.Lerp(0.5f, 1f, (currentSpeed - _walkSpeed) / (_runSpeed - _walkSpeed));
            }
            return 1f;
        }
    }
}