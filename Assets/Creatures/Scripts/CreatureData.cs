using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Data/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("LOCOMOTION")]
    public float RotationSpeed;
    public float WalkSpeed;
    public float RunSpeed;

    [Header("DISTANCES")]
    public float AttackDistance;
    public float RunDistance;

    [Header("VALUES")]
    public int Souls;
    public float Focus;
    public float Health;

    [Header("REPOSITIONING")]
    public int RetreatPoints = 0;
    public float RetreatRadius = 0;
    public float RetreatDegrees = 360;
}