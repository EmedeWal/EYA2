using UnityEngine;

[CreateAssetMenu(fileName = "New Player Attack Data", menuName = "Player Attack Data", order = 51)]
public class PlayerAttackData : ScriptableObject
{
    [Header("GENERAL")]
    public LayerMask EnemyLayer;

    [Header("TIMING")]
    public float ChargeDuration;
    public float AttackSpeed;
    public float AttackDuration;

    [Header("LUNGING")]
    public float LungeSpeed;
    public float LungeDuration;

    [Header("LETHALITY")]
    public Vector3 AttackSize;
    public int AttackDamage;

    [Header("AUDIO")]
    public AudioClip AttackClip;
}