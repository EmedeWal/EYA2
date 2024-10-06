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
    private bool _playerInside = false;

    public void Init(PlayerStats playerStats, float criticalChanceBonus, float criticalMultiplierBonus, float damageReductionBonus, float healthRegenBonus)
    {
        _playerStats = playerStats;

        _criticalChanceBonus = criticalChanceBonus;
        _criticalMultiplierBonus = criticalMultiplierBonus;
        _damageReductionBonus = damageReductionBonus;
        _healthRegenBonus = healthRegenBonus;

        _statChanges.Add(Stat.CriticalChance, _criticalChanceBonus);
        _statChanges.Add(Stat.CriticalMultiplier, _criticalMultiplierBonus);
        _statChanges.Add(Stat.DamageReduction, _damageReductionBonus);
        _statChanges.Add(Stat.HealthRegen, _healthRegenBonus);
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
            _playerInside = true;

            IncrementStat(Stat.CriticalChance, _criticalChanceBonus);
            IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierBonus);
            IncrementStat(Stat.DamageReduction, _damageReductionBonus * _enemyCounter);
            IncrementStat(Stat.HealthRegen, _healthRegenBonus * _enemyCounter);
        }

        if (other.CompareTag("Enemy"))
        {
            _enemyCounter++;

            if (_playerInside)
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
            _playerInside = false;

            ResetStatChanges();
        }

        if (other.CompareTag("Enemy"))
        {
            _enemyCounter--;

            //if (_playerInside)
            //{
            //    IncrementStat(Stat.DamageReduction, -_damageReductionBonus);
            //    IncrementStat(Stat.HealthRegen, -_healthRegenBonus);
            //}
        }
    }

    private void IncrementStat(Stat stat, float value)
    {
        _statChanges[stat] += value;
        _playerStats.IncrementStat(stat, value);
    }

    private void ResetStatChanges()
    {
        List<KeyValuePair<Stat, float>> statChangeList = new List<KeyValuePair<Stat, float>>(_statChanges);

        foreach (var statChange in statChangeList)
        {
            _playerStats.IncrementStat(statChange.Key, -statChange.Value);
        }
    }
}
