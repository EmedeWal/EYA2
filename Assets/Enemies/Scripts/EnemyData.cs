using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        [CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Data/EnemyData")]
        public class EnemyData : ScriptableObject
        {
            [Header("VALUES")]
            public int Souls;
            public int Health;
            public int Posture;
            public int PostureRecovery;
            public float LockPointOffset = 1f;

            [Header("OFFENSE")]
            public float MaxAngle;

            [Header("DISTANCES")]
            public float AttackDistance;
            public float RunDistance;

            [Header("LOCOMOTION")]
            public bool KeepCombatLocomotion = false;
            public float WalkSpeed;
            public float RunSpeed;
        }
    }
}