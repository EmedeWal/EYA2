using UnityEngine;

public class ChasingState : CreatureState
{
    private Transform _target;
    private float _walkDistance;
    private float _runDistance;
    private float _maxLocomotionSpeed;

    public ChasingState(CreatureAI creatureAI, Transform target) : base(creatureAI)
    {
        _target = target;
        _walkDistance = _CreatureAI.CreatureData.WalkDistance;
        _runDistance = _CreatureAI.CreatureData.RunDistance;    
        _maxLocomotionSpeed = _CreatureAI.Locomotion.GetMaxSpeed(); 
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

    }

    private void UpdateLocomotion(float distanceToTarget)
    {
        float speed = 0;

        if (distanceToTarget <= _runDistance)
        {
            speed = _CreatureAI.CreatureData.RunSpeed;
        }
        else if (distanceToTarget <= _walkDistance)
        {
            speed = _CreatureAI.CreatureData.WalkSpeed;
        }

        _CreatureAI.Locomotion.SetSpeed(speed);
    }
}
