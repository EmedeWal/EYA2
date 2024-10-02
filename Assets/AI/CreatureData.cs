using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Data/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("AGENT")]
    public float MovementSpeed;

    [Header("DISTANCE")]
    public float ChaseDistance;

    [Header("COMBAT")]
    public float Recovery;
    public float Health;
}
