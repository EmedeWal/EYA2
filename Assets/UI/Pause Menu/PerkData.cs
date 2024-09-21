using UnityEngine;

[CreateAssetMenu(fileName = "PerkData", menuName = "ScriptableObjects/PerkData")]
public class PerkData : ScriptableObject
{
    public string Title;
    public string Description;
    public bool IsUnlocked;
}
