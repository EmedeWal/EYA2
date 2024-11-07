using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        public class IdleState : EnemyState
        {
            private Transform _target;

            public IdleState(Enemy enemy) : base(enemy)
            {
                if (!_Enemy.EnemyData.KeepCombatLocomotion)
                {
                    _Enemy.AnimatorManager.SetBool("InCombat", false);
                }
            }

            public override void Tick(float delta)
            {
                Transform nearestTarget = _Enemy.GetNearestTarget(_Enemy.EnemyData.RunDistance);
                Transform defaultTarget = _Enemy.DefaultTarget;

                if (nearestTarget != null)
                {
                    _target = nearestTarget;
                }
                else
                {
                    _target = defaultTarget;
                }

                _Enemy.SetState(new ChasingState(_Enemy, _target));
            }

            public override void Exit()
            {
                _Enemy.DetermineAttack(_target);
            }
        }
    }
}