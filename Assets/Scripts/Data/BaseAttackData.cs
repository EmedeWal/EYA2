using UnityEngine;

namespace EmeWillem
{
    [CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Data/Attack Data/Attack")]
    public class BaseAttackData : ScriptableObject
    {
        [HideInInspector] public OffenseCollider OffenseCollider;

        [Header("DAMAGE")]
        public int Damage;
        public int Stagger;

        [Header("HITBOX")]
        public OffenseColliderType OffenseColliderType;

        [Header("AUDIO")]
        public AudioClip AudioClip;
        public float AudioOffset;
        public float AudioVolume;

        [Header("CREATURE RELATED")]
        public AttackType AttackType;
        public AttackMode AttackMode;
        public Vector3 AttackOffset;
        public Vector3 AttackHitBox;
        public float AttackRadius;
        public float Distance = 0;
        public float Recovery = 1;
        public string AnimationName;

        public virtual void Attack(Transform target) { }
    }
}