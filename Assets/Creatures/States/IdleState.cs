namespace EmeWillem
{
    using UnityEngine;
    public class IdleState : CreatureState
    {
        private Transform _target;

        public IdleState(CreatureAI creatureAI) : base(creatureAI)
        {
            if (!_CreatureAI.CreatureData.KeepCombatLocomotion)
            {
                _CreatureAI.AnimatorManager.SetBool("InCombat", false);
            }
        }

        public override void Tick(float delta)
        {
            Transform nearestTarget = _CreatureAI.GetNearestTarget(_CreatureAI.CreatureData.RunDistance);
            Transform defaultTarget = _CreatureAI.DefaultTarget;

            if (nearestTarget != null)
            {
                _target = nearestTarget;
            }
            else if (defaultTarget != null)
            {
                _target = defaultTarget;
            }

            if (_target != null)
            {
                _CreatureAI.SetState(new ChasingState(_CreatureAI, _target));
            }
        }

        public override void Exit()
        {
            _CreatureAI.DetermineAttack(_target);
        }
    }
}