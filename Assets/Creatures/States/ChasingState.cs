using UnityEngine;
using UnityEngine.AI;

public class ChasingState : CreatureState
{
    private Transform _target;
    private float _runDistance;
    private float _maxLocomotionSpeed;

    public ChasingState(CreatureAI creatureAI, Transform target) : base(creatureAI)
    {
        _target = target;
        _runDistance = _CreatureAI.CreatureData.RunDistance;    
        _maxLocomotionSpeed = _CreatureAI.Locomotion.GetMaxSpeed();
        _CreatureAI.Health.ValueRemoved += ChasingState_ValueRemoved;
    }

    public override void Enter()
    {
        _CreatureAI.Locomotion.StopAgent(false);
    }

    public override void Tick(float delta)
    {
        float distanceToTarget = Vector3.Distance(_CreatureAI.transform.position, _target.position);

        UpdateLocomotion(distanceToTarget);

        _CreatureAI.Locomotion.SetDestination(_target.position);

        if (_CreatureAI.TargetInRange(_target))
        {
            _CreatureAI.SetState(new AttackingState(_CreatureAI, _target));
        }
    }

    public override void Exit()
    {
        _CreatureAI.Health.ValueRemoved -= ChasingState_ValueRemoved;
    }

    private void UpdateLocomotion(float distanceToTarget)
    {
        float speed;

        if (distanceToTarget <= _runDistance)
        {
            speed = _CreatureAI.CreatureData.RunSpeed;
        }
        else
        {
            speed = _CreatureAI.CreatureData.WalkSpeed;
        }

        _CreatureAI.Locomotion.SetSpeed(speed);
    }

    private void ChasingState_ValueRemoved()
    {
        _CreatureAI.SetState(new IdleState(_CreatureAI));
    }
}