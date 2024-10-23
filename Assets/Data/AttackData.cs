using UnityEngine;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Data/Attack Data/Attack")]
public class AttackData : ScriptableObject
{
    [Header("ENUM")]
    public AttackType AttackType;
    public AttackMode AttackMode;

    [Header("ANIMATION")]
    public string AnimationName;

    [Header("AUDIO")]
    public AudioClip AudioClip;
    public float AudioOffset;
    public float AudioVolume;

    [Header("DAMAGE")]
    public float Damage;

    [Header("MISC")]
    public float Distance = 0;
    public float Recovery = 1;

    [Header("HITBOX")]
    public Vector3 AttackOffset;
    public Vector3 AttackHitBox;
    public float AttackRadius;

    public virtual void Attack(Transform target) { }
}