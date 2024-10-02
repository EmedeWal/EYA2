using UnityEngine;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Data/Attack Data", order = 51)]
public class AttackData : ScriptableObject
{
    [Header("ANIMATION")]
    public string AnimationName;

    [Header("AUDIO")]
    public AudioClip AttackClip;

    [Header("DAMAGE")]
    public float Damage;

    [Header("HITBOX")]
    public Vector3 AttackOffset;
    public float AttackRadius;
}