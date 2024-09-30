using UnityEngine;

[CreateAssetMenu(fileName = "StatIncreasePerkData", menuName = "Perks/StatIncreasePerk")]
public class StatIncreasePerkData : PerkData
{
    [Header("STAT INCREASE DETAILS")]
    public PlayerStats PlayerStats;
    public Stat TargetStat; 
    public float Increment;

    public override void Activate()
    {
        PlayerStats.IncrementStat(TargetStat, Increment);
    }

    public override void Deactivate()
    {
        PlayerStats.IncrementStat(TargetStat, -Increment);
    }
}
