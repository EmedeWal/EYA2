using UnityEngine;
using UnityEngine.AI;

public class CirclingState : CreatureState
{
    private Transform _target;
    private Vector3 _destination;
    private float _reachedThreshold = 0.2f;

    public CirclingState(CreatureAI creature, Transform target) : base(creature)
    {
        _target = target;
    }

    public override void Enter()
    {
        _CreatureAI.Locomotion.StopAgent(false);
        _CreatureAI.Locomotion.UpdateRotation(false);
        _CreatureAI.AnimatorManager.SetBool("InCombat", true);
        _CreatureAI.Locomotion.SetSpeed(_CreatureAI.CreatureData.WalkSpeed);

        Vector3 directionToPlayer = (_target.position - _CreatureAI.transform.position).normalized;
        float angleOffset = Random.value < 0.5f ? -90f : 90f;

        Vector3 circleDirection = Quaternion.Euler(0, angleOffset, 0) * directionToPlayer;
        Vector3 desiredPosition = _CreatureAI.transform.position + circleDirection * 2f;

        if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            _destination = hit.position;
            _CreatureAI.Locomotion.SetDestination(_destination);
        }
        else
        {
            _CreatureAI.SetState(new IdleState(_CreatureAI));
        }
    }

    public override void Tick(float delta)
    {
        Vector3 directionToPlayer = (_target.position - _CreatureAI.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        _CreatureAI.transform.rotation = Quaternion.Slerp(_CreatureAI.transform.rotation, targetRotation, _CreatureAI.CreatureData.RotationSpeed * delta);

        if (Vector3.Distance(_CreatureAI.transform.position, _destination) <= _reachedThreshold)
        {
            _CreatureAI.SetState(new IdleState(_CreatureAI));
        }
    }

    public override void Exit()
    {
        _CreatureAI.AnimatorManager.SetBool("InCombat", false);
        _CreatureAI.Locomotion.UpdateRotation(true);
    }
}