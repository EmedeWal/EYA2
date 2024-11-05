using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Data/Attack Data/Attack (Player)")]
    public class AttackData : BaseAttackData
    {
        [Header("ANIMATION")]
        public int AnimationHash;
    }
}