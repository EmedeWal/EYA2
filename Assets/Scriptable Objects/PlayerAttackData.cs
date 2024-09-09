using UnityEngine;

[CreateAssetMenu(fileName = "New Player Attack Data", menuName = "Player Attack Data", order = 51)]
public class PlayerAttackData : ScriptableObject
{
    [Header("ANIMATION")]
    public string AnimationName;

    [Header("AUDIO")]
    public AudioClip AttackClip;

    [Header("LETHALITY")]
    public Vector3 AttackSize;
    public int AttackDamage;

    [Header("LUNGING")]
    public float LungeForce;
}