using UnityEngine.AI;
using UnityEngine;

public class CreatureLocomotion : MonoBehaviour, IMovingProvider
{
    public bool Moving { get; private set; }

    private CreatureAnimatorManager _animatorManager;
    private NavMeshAgent _navMeshAgent;
    private float _maxSpeed;

    public void Init(float speed)
    {
        _animatorManager = GetComponent<CreatureAnimatorManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = speed;
        _maxSpeed = speed;
    }

    public void SetDestination(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    public void StopAgent(bool stop)
    {
        _navMeshAgent.isStopped = stop;
        Moving = !stop;
    }

    public void UpdateRotation(bool update)
    {
        _navMeshAgent.updateRotation = update;
    }

    public void SetSpeed(float speed)
    {
        speed *= _animatorManager.MovementSpeed;
        _navMeshAgent.speed = speed;
    }

    public float GetLocomotionValue()
    {
        return _navMeshAgent.velocity.magnitude / _animatorManager.MovementSpeed / _maxSpeed;
    }

    public float GetMaxSpeed()
    {
        return _maxSpeed;
    }
}