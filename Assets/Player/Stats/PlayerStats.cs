using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    private Dictionary<Stat, float> _baseStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> _currentStats = new Dictionary<Stat, float>();

    public event Action<Stat, float> StatChanged;

    public void Init()
    {
        _baseStats.Clear();

        _baseStats.Add(Stat.MaxHealth, 100f);
        _baseStats.Add(Stat.MaxMana, 100f);
        _baseStats.Add(Stat.BaseMovementSpeed, 5f);
        _baseStats.Add(Stat.BaseLightAttackDamage, 10f);
        _baseStats.Add(Stat.BaseHeavyAttackDamage, 20f);

        _currentStats.Clear();

        _currentStats.Add(Stat.CurrentHealth, _baseStats[Stat.MaxHealth]);
        _currentStats.Add(Stat.CurrentMana, _baseStats[Stat.MaxMana]);
        _currentStats.Add(Stat.HealthRegen, 1f);
        _currentStats.Add(Stat.ManaRegen, 1f);
        _currentStats.Add(Stat.CriticalChance, 0f);
        _currentStats.Add(Stat.CriticalMultiplier, 2f);
        _currentStats.Add(Stat.DamageReduction, 0f);
        _currentStats.Add(Stat.EvasionChance, 0f);
        _currentStats.Add(Stat.StaggerMultiplier, 1f);
        _currentStats.Add(Stat.MovementSpeedModifier, 1f);
        _currentStats.Add(Stat.AttackDamageModifier, 1f);
        _currentStats.Add(Stat.LightAttackDamageModifier, 1f);
        _currentStats.Add(Stat.HeavyAttackDamageModifier, 1f);

        RecalculateTotal(Stat.MovementSpeed);
        RecalculateTotal(Stat.LightAttackDamage);
        RecalculateTotal(Stat.HeavyAttackDamage);
    }

    public void IncrementStat(Stat stat, float amount)
    {
        if (_currentStats.ContainsKey(stat))
        {
            _currentStats[stat] += amount;

            if (stat == Stat.BaseLightAttackDamage || stat == Stat.AttackDamageModifier || stat == Stat.LightAttackDamageModifier)
            {
                RecalculateTotal(Stat.LightAttackDamage);
            }

            if (stat == Stat.BaseHeavyAttackDamage || stat == Stat.AttackDamageModifier || stat == Stat.HeavyAttackDamageModifier)
            {
                RecalculateTotal(Stat.HeavyAttackDamage);
            }

            if (stat == Stat.MovementSpeedModifier || stat == Stat.BaseMovementSpeed)
            {
                RecalculateTotal(Stat.MovementSpeed);
            }

            OnStatChanged(stat, _currentStats[stat]);
        }
        else
        {
            Debug.LogWarning($"Stat {stat} not found in dictionary.");
        }
    }

    public float GetStat(Stat stat)
    {
        if (_currentStats.ContainsKey(stat))
        {
            return _currentStats[stat];
        }
        Debug.LogWarning($"Stat {stat} not found in dictionary.");
        return 0f;
    }

    private void RecalculateTotal(Stat totalStat)
    {
        if (totalStat == Stat.MovementSpeed)
        {
            float baseMovementSpeed = _baseStats[Stat.BaseMovementSpeed];
            float movementModifier = _currentStats[Stat.MovementSpeedModifier];

            float totalMovementSpeed = baseMovementSpeed * movementModifier;

            _currentStats[Stat.MovementSpeed] = totalMovementSpeed;
            OnStatChanged(Stat.MovementSpeed, totalMovementSpeed);
        }

        if (totalStat == Stat.LightAttackDamage)
        {
            float baseDamage = _baseStats[Stat.BaseLightAttackDamage];
            float attackModifier = _currentStats[Stat.AttackDamageModifier];
            float lightModifier = _currentStats[Stat.LightAttackDamageModifier];

            float totalLightDamage = baseDamage * attackModifier * lightModifier;

            _currentStats[Stat.LightAttackDamage] = totalLightDamage;
            OnStatChanged(Stat.LightAttackDamage, totalLightDamage);
        }

        if (totalStat == Stat.HeavyAttackDamage)
        {
            float baseDamage = _baseStats[Stat.BaseHeavyAttackDamage];
            float attackModifier = _currentStats[Stat.AttackDamageModifier];
            float heavyModifier = _currentStats[Stat.HeavyAttackDamageModifier];

            float totalHeavyDamage = baseDamage * attackModifier * heavyModifier;

            _currentStats[Stat.HeavyAttackDamage] = totalHeavyDamage;
            OnStatChanged(Stat.HeavyAttackDamage, totalHeavyDamage);
        }
    }

    private void OnStatChanged(Stat stat, float value)
    {
        StatChanged?.Invoke(stat, value);
    }
}
