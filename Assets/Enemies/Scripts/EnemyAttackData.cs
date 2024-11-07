using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        [CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Data/Attack Data/Enemy Attack")]
        public class AttackData : BaseAttackData
        {
            [Header("ANIMATION")]
            public string AnimationName;

            [Header("ENUMS")]
            public AttackType AttackType;

            [Header("SETTNGS")]
            public float RotationSpeed;
            public float DistanceUnits;
            public float RecoveryTime;
        }
    }
}