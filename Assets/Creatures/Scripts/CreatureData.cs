using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Data/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("VALUES")]
    public int Souls;
    public float Health;

    [Header("STAGGER")]
    public float StaggerRecovery;
    public float StaggerThreshold;

    [Header("OFFENSE")]
    public float MaxAngle;

    [Header("DISTANCES")]
    public float AttackDistance;
    public float RunDistance;

    [Header("LOCOMOTION")]
    public float RotationSpeed;
    public float WalkSpeed;
    public float RunSpeed;
}