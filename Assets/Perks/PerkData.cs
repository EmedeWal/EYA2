using UnityEngine;

[CreateAssetMenu(fileName = "PerkData", menuName = "PerkData")]
public class PerkData : ScriptableObject
{
    [Header("INFORMATION")]
    public string Title;
    public string Description;

    [Header("STATISTICS")]
    public int Tier;
    public int Cost;
}
