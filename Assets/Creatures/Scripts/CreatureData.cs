using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Data/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("LOCOMOTION")]
    public float RotationSpeed;
    public float WalkSpeed;
    public float RunSpeed;

    [Header("DISTANCES")]
    public float RetreatDistance;
    public float AttackDistance;
    public float WalkDistance;
    public float RunDistance;

    [Header("VALUES")]
    public int Souls;
    public float Health;
}