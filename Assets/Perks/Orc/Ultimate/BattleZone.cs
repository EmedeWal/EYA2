using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    private Dictionary<Stat, float> _statChanges = new();
    private PlayerStats _playerStats;

    private float _criticalChanceBonus = 0;
    private float _criticalMultiplierBonus = 0;
    private float _damageReductionBonus = 0;
    private float _healthRegenBonus = 0;

    private int _enemyCounter = 0;

    public bool PlayerInside { get; private set; } = false;

    public void Init(PlayerStats playerStats, float criticalChanceBonus, float criticalMultiplierBonus, float damageReductionBonus, float healthRegenBonus)
    {
        _playerStats = playerStats;

        _criticalChanceBonus = criticalChanceBonus;
        _criticalMultiplierBonus = criticalMultiplierBonus;
        _damageReductionBonus = damageReductionBonus;
        _healthRegenBonus = healthRegenBonus;

        _statChanges.Add(Stat.CriticalChance, 0);
        _statChanges.Add(Stat.CriticalMultiplier, 0);
        _statChanges.Add(Stat.DamageReduction, 0);
        _statChanges.Add(Stat.HealthRegen, 0);
    }

    public void Cleanup()
    {
        ResetStatChanges();
        _statChanges.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInside = true;

            IncrementStat(Stat.CriticalChance, _criticalChanceBonus);
            IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierBonus);
            IncrementStat(Stat.DamageReduction, _damageReductionBonus * _enemyCounter);
            IncrementStat(Stat.HealthRegen, _healthRegenBonus * _enemyCounter);
        }

        if (other.CompareTag("Enemy"))
        {
            _enemyCounter++;

            if (PlayerInside)
            {
                IncrementStat(Stat.DamageReduction, _damageReductionBonus);
                IncrementStat(Stat.HealthRegen, _healthRegenBonus);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInside = false;
            ResetStatChanges();
        }

        if (other.CompareTag("Enemy"))
        {
            _enemyCounter--;

            if (PlayerInside)
            {
                IncrementStat(Stat.DamageReduction, -_damageReductionBonus);
                IncrementStat(Stat.HealthRegen, -_healthRegenBonus);
            }
        }
    }

    private void IncrementStat(Stat stat, float value)
    {
        _statChanges[stat] += value;
        _playerStats.IncrementStat(stat, value);
    }

    private void ResetStatChanges()
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
}
