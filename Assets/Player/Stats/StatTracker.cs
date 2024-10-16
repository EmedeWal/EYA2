using System.Collections.Generic;

public class StatTracker 
{
    private Dictionary<Stat, float> _statChanges;
    private PlayerStats _playerStats;

    public StatTracker (Dictionary<Stat, float> statChanges, PlayerStats playerStats)
    {
        _statChanges = statChanges;
        _playerStats = playerStats;
    }

    public void IncrementStat(Stat stat, float value)
    {
        _statChanges[stat] += value;
        _playerStats.IncrementStat(stat, value);
    }

    public void ResetStatChanges()
    {
        var statChangeCopy = new Dictionary<Stat, float>(_statChanges);

        foreach (var statChange in statChangeCopy)
        {
            if (statChange.Value != 0)
            {
                _playerStats.IncrementStat(statChange.Key, -statChange.Value);
                _statChanges[statChange.Key] = 0;
            }
        }
    }

    public float GetStatChange(Stat stat)
    {
        return _statChanges[stat];    
    }
}
