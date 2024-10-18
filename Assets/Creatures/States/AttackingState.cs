using UnityEngine;

public class AttackingState : CreatureState
{
    private float _delta;

    private Transform _target;
    private bool _shouldRotate;

    public AttackingState(CreatureAI creatureAI, Transform target) : base(creatureAI)
    {
        _target = target;
        _shouldRotate = false;
    }

    public override void Enter()
    {
        _CreatureAI.Locomotion.StopAgent(true);

        _CreatureAI.AttackHandler.AttackBegun += AttackingState_AttackBegun;
        _CreatureAI.AttackHandler.AttackHalfway += AttackingState_AttackHalfway;
        _CreatureAI.AttackHandler.AttackEnded += AttackingState_AttackEnded;
    }

    public override void Tick(float delta)
    {        
        _delta = delta;

        if (_target != null)
        {
            if (_CreatureAI.AttackHandler.IsAttacking)
            {
                if (_shouldRotate)
                {
                    RotateTowardsTarget(delta);
                }
            }
            else
            {
                if (_CreatureAI.TargetInRange(_target))
                {
                    _CreatureAI.AttackHandler.Attack();
                }
                else
                {
                    _CreatureAI.SetState(new IdleState(_CreatureAI));
                }
            }
        }
        else
        {
            _CreatureAI.AnimatorManager.Animator.SetBool("IsAttacking", false);
            _CreatureAI.SetState(new IdleState(_CreatureAI));
        }
    }

    public override void Exit()
    {
        _CreatureAI.AttackHandler.AttackBegun -= AttackingState_AttackBegun;
        _CreatureAI.AttackHandler.AttackHalfway -= AttackingState_AttackHalfway;
        _CreatureAI.AttackHandler.AttackEnded -= AttackingState_AttackEnded;
    }

    private void AttackingState_AttackBegun(AttackType attackType)
    {
        _shouldRotate = true; 
    }

    private void AttackingState_AttackHalfway(AttackType attackType)
    {
        _shouldRotate = false; 
    }

    private void AttackingState_AttackEnded(AttackType attackType)
    {
        _CreatureAI.AnimatorManager.CrossFadeAnimation(_delta, "Combat Idle");

        if (attackType == AttackType.Heavy && _CreatureAI.CreatureData.RetreatRadius > 0 && IsPlayerBehind())
        {
            _CreatureAI.AnimatorManager.Animator.SetBool("IsAttacking", false);
            _CreatureAI.SetState(new RepositioningState(_CreatureAI, _target));
        }
    }

    private void RotateTowardsTarget(float delta)
    {
        Vector3 direction = (_target.position - _CreatureAI.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        _CreatureAI.transform.rotation = Quaternion.Slerp(_CreatureAI.transform.rotation, lookRotation, delta * _CreatureAI.CreatureData.RotationSpeed);
    }

    private bool IsPlayerBehind()
    {
        Vector3 directionToTarget = (_target.position - _CreatureAI.transform.position).normalized;
        float angleToTarget = Vector3.Angle(_CreatureAI.transform.forward, directionToTarget);
        return angleToTarget > 90;
    }
}