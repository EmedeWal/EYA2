using UnityEngine;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Data/Attack Data/Attack")]
public class BaseAttackData : ScriptableObject
{
    [HideInInspector] public HitCollider HitCollider;

    [Header("DAMAGE")]
    public float Damage;

    [Header("HITBOX")]
    public HitBoxType HitBoxType;

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