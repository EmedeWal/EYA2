using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Melee Data", menuName = "Scriptable Object/Data/Enemy Melee Data", order = 51)]
public class EnemyMeleeData : ScriptableObject
{
    [Header("TIMING")]
    public float ChargeDuration;
    public float AttackDuration;
    public float AttackCooldown;

    [Header("LETHALITY")]
    public Vector3 AttackSize;
    public int AttackDamage;
    public float AttackRange;

    [Header("ANIMATION")]
    public string AnimationParameter;

    [Header("AUDIO")]
    public AudioClip AttackClip;
}
