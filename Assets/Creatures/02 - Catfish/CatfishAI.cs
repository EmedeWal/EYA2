using UnityEngine;

namespace EmeWillem
{
    public class CatfishAI : CreatureAI
    {
        [Header("VARIABLES")]
        [SerializeField] private int _retreatPoints = 9;
        [SerializeField] private float _retreatRadius = 3;
        [SerializeField] private float _retreatDegrees = 360;

        public override void DetermineBehavior(BaseAttackData attackData, Transform target)
        {
            if (attackData.AttackType == AttackType.Heavy && IsTargetBehind(target))
            {
                SetState(new RepositioningState(this, target, _retreatPoints, _retreatRadius, _retreatDegrees));
            }
            else
            {
                base.DetermineBehavior(attackData, target);
            }
        }
    }
}