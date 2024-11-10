using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        public class AttackingState : EnemyState
        {
            private float _delta;

            private AttackData _attackData;
            private AnimatorManager _animatorManager;
            private AttackHandler _attackHandler;
            private Locomotion _locomotion;
            private Transform _transform;
            private Transform _target;
            private int _canRotateHash;
            private int _inActionHash;
            private int _inCombatHash;
            private float _cooldownTimer;
            private bool _onCooldown;

            public AttackingState(Enemy enemy, Transform target) : base(enemy)
            {
                _animatorManager = _Enemy.AnimatorManager;
                _attackHandler = _Enemy.AttackHandler;
                _locomotion = _Enemy.Locomotion;
                _transform = _Enemy.Transform;
                _target = target;
                _canRotateHash = Animator.StringToHash("CanRotate");
                _inActionHash = Animator.StringToHash("InAction");
                _inCombatHash = Animator.StringToHash("InCombat");
                _cooldownTimer = 0;
                _onCooldown = false;
            }

            public override void Enter()
            {
                _attackHandler.EnteredAttackingState += AttackingState_EnteredAttackingState;
                _attackHandler.LeftAttackingState += AttackingState_LeftAttackingState;

                _animatorManager.SetBool(_inCombatHash, true);
                _locomotion.StopAgent(true);
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
                    if (_animatorManager.GetBool(_inActionHash))
                    {
                        if (_animatorManager.GetBool(_canRotateHash))
                        {
                            RotateTowardsTarget(delta);
                        }
                        else
                        {
                            _locomotion.StopAgent(true);
                        }
                    }
                    else
                    {
                        if (_Enemy.IsTargetInRange(_target))
                        {
                            _animatorManager.CrossFade(_attackHandler.AttackData.AnimationName);
                        }
                        else
                        {
                            _Enemy.SetState(new IdleState(_Enemy));
                        }
                    }
                }
                else
                {
                    _Enemy.SetState(new IdleState(_Enemy));
                }
            }

            public override void Exit()
            {
                _attackHandler.EnteredAttackingState -= AttackingState_EnteredAttackingState;
                _attackHandler.LeftAttackingState -= AttackingState_LeftAttackingState;
            }

            private void AttackingState_EnteredAttackingState(BaseAttackData baseAttackData)
            {
                AttackData attackData = baseAttackData as AttackData;

                _attackData = attackData;
                if (attackData.DistanceUnits > 0)
                {
                    float distanceToTarget = Vector3.Distance(_transform.position, _target.position);
                    if (distanceToTarget > _Enemy.EnemyData.AttackDistance)
                    {
                        Vector3 attackTargetPosition = _transform.position + _transform.forward * attackData.DistanceUnits;

                        _locomotion.SetDestination(attackTargetPosition);
                        _locomotion.StopAgent(false);
                    }
                }
            }

            private void AttackingState_LeftAttackingState(BaseAttackData baseAttackData)
            {
                AttackData attackData = baseAttackData as AttackData;

                _Enemy.DetermineBehavior(attackData, _target);
                _cooldownTimer = attackData.RecoveryTime;
                _onCooldown = true;
            }

            private void RotateTowardsTarget(float delta)
            {
                Vector3 direction = (_target.position - _transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, delta * _attackData.RotationSpeed);
                }
            }
        }
    }
}