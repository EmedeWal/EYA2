using Unity.VisualScripting;
using UnityEngine;

public class AttackingState : CreatureState
{
    private float _delta;

    private Transform _target;
    private float _cooldownTimer;
    private bool _onCooldown;
    private bool _canRotate;

    public AttackingState(CreatureAI creatureAI, Transform target) : base(creatureAI)
    {
        _target = target;
        _cooldownTimer = 0;
        _onCooldown = false;
        _canRotate = false;
    }

    public override void Enter()
    {
        _CreatureAI.Locomotion.StopAgent(true);
        _CreatureAI.AnimatorManager.SetBool("InCombat", true);

        _CreatureAI.AttackHandler.AttackBegun += AttackingState_AttackBegun;
        _CreatureAI.AttackHandler.AttackHalfway += AttackingState_AttackHalfway;
        _CreatureAI.AttackHandler.AttackEnded += AttackingState_AttackEnded;
    }

    public override void Tick(float delta)
    {
        _delta = delta;

        if (_onCooldown)
        {
            _cooldownTimer -= delta;
            if (_cooldownTimer <= 0)
            {
                _onCooldown = false;
            }
        }
        else if (_target != null)
        {
            if (_CreatureAI.AnimatorManager.GetBool("InAction"))
            {
                if (_canRotate)
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
            _CreatureAI.SetState(new IdleState(_CreatureAI));
        }
    }

    public override void Exit()
    {
        _CreatureAI.AttackHandler.AttackBegun -= AttackingState_AttackBegun;
        _CreatureAI.AttackHandler.AttackHalfway -= AttackingState_AttackHalfway;
        _CreatureAI.AttackHandler.AttackEnded -= AttackingState_AttackEnded;

        _CreatureAI.AnimatorManager.SetBool("InCombat", false);
    }

    private void AttackingState_AttackBegun(AttackData attackData)
    {
        AttackMode attackMode = attackData.AttackMode;

        if (attackMode == AttackMode.Lunging)
        {
            float distanceToTarget = Vector3.Distance(_CreatureAI.Transform.position, _target.position);
            if (distanceToTarget > _CreatureAI.CreatureData.AttackDistance)
            {
                Vector3 attackTargetPosition = _CreatureAI.Transform.position + _CreatureAI.transform.forward * attackData.Distance;

                _CreatureAI.Locomotion.SetDestination(attackTargetPosition);
                _CreatureAI.Locomotion.StopAgent(false);
            }
        }
        else if (attackMode == AttackMode.Tracking)
        {
            _canRotate = true;
        }
    }

    private void AttackingState_AttackHalfway(AttackData attackData)
    {
        AttackMode attackMode = attackData.AttackMode;

        if (attackMode == AttackMode.Lunging)
        {
            _CreatureAI.Locomotion.StopAgent(true);
        }
        else if (attackMode == AttackMode.Tracking)
        {
            _canRotate = false;
        }
    }

    private void AttackingState_AttackEnded(AttackData attackData)
    {
        _CreatureAI.AttackHandler.ChooseAttack(_target, _CreatureAI.CreatureData.MaxAngle);

        if (attackData.AttackType == AttackType.Heavy && _CreatureAI.CreatureData.RetreatRadius > 0 && IsPlayerBehind())
        {
            _CreatureAI.SetState(new RepositioningState(_CreatureAI, _target));
        }
        else
        {
            _cooldownTimer = attackData.Recovery;
            _onCooldown = true;
        }
    }

    private void RotateTowardsTarget(float delta)
    {
        Vector3 direction = (_target.position - _CreatureAI.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _CreatureAI.transform.rotation = Quaternion.Slerp(_CreatureAI.transform.rotation, lookRotation, delta * _CreatureAI.CreatureData.RotationSpeed);
        }
    }

    private bool IsPlayerBehind()
    {
        Vector3 directionToTarget = (_target.position - _CreatureAI.transform.position).normalized;
        float angleToTarget = Vector3.Angle(_CreatureAI.transform.forward, directionToTarget);
        return angleToTarget > 90;
    }
}