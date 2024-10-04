using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Data/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("AGENT")]
    public float MovementSpeed;
    public float MovementModifier;

    [Header("DISTANCE")]
    public float ChaseDistance;

    [Header("DETECTTION")]
    public float DetectionDistance;

    [Header("COMBAT")]
    public float Health;
    public float AttackSpeed;
    public float DamageModifier;
}
