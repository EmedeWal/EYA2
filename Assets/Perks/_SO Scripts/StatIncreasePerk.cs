using UnityEngine;

[CreateAssetMenu(fileName = "StatIncreasePerkData", menuName = "Scriptable Object/Perks/Stat Increase")]
public class StatIncreasePerkData : PerkData
{
    [Header("STAT INCREASE DETAILS")]
    [SerializeField] private Stat _targetStat; 
    [SerializeField] private float _increment;

    public override void Activate()
    {
        _PlayerStats.IncrementStat(_targetStat, _increment);
    }

    public override void Deactivate()
    {
        _PlayerStats.IncrementStat(_targetStat, -_increment);
    }
}
