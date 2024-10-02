using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Data/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("AGENT STATS")]
    public float MovementSpeed;

    [Header("COMBAT STATS")]
    public float Health;
}
