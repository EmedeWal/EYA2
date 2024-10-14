using UnityEngine.AI;
using UnityEngine;

public class CreatureLocomotion : MonoBehaviour, IMovingProvider
{
    public bool Moving {  get; private set; }

    private NavMeshAgent _navMeshAgent;
    private float _maxSpeed;

    public void Init(float speed)
    {
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

    public void SetSpeed(float speed)
    {
        _navMeshAgent.speed = speed;
    }

    public float GetLocomotionValue()
    {
        return _navMeshAgent.velocity.magnitude / _maxSpeed;
    }

    public float GetMaxSpeed()
    {
        return _navMeshAgent.speed;
    }
}