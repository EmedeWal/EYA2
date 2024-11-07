using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        public class ChasingState : EnemyState
        {
            private EnemyData _enemyData;
            private Locomotion _locomotion;
            private Transform _transform;
            private Transform _target;

            public ChasingState(Enemy enemy, Transform target) : base(enemy)
            {
                _enemyData = _Enemy.EnemyData;
                _locomotion = _Enemy.Locomotion;
                _transform = _Enemy.Transform;

                _target = target;
            }

            public override void Enter()
            {
                _locomotion.StopAgent(false);
            }

            public override void Tick(float delta)
            {
                if (_Enemy.AnimatorManager.GetBool("InAction"))
                {
                    _locomotion.StopAgent(true);
                }
                else if (_target != null)
                {
                    float distanceToTarget = Vector3.Distance(_transform.position, _target.position);
                    UpdateLocomotion(distanceToTarget);
                    _locomotion.StopAgent(false);

                    if (_Enemy.IsTargetInRange(_target))
                    {
                        _Enemy.SetState(new AttackingState(_Enemy, _target));
                    }
                    else
                    {
                        _locomotion.SetDestination(_target.position);
                    }
                }
                else
                {
                    _Enemy.SetState(new IdleState(_Enemy));
                }
            }

            private void UpdateLocomotion(float distanceToTarget)
            {
                float speed;

                if (distanceToTarget <= _enemyData.RunDistance)
                {
                    speed = _enemyData.RunSpeed;
                }
                else
                {
                    speed = _enemyData.WalkSpeed;
                }

                _locomotion.SetSpeed(speed);
            }
        }
    }
}