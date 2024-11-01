using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Object/Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    private Dictionary<Stat, float> _baseStats;
    private Dictionary<Stat, float> _currentStats;

    public event Action<Stat, float> StatChanged;

    public void Init()
    {
        _baseStats = new()
        {
            { Stat.MaxHealth, 100f },
            { Stat.MaxMana, 100f },
            { Stat.BaseMovementSpeed, 5f },
            { Stat.AttackSpeed, 1f },
            { Stat.BaseLightAttackDamage, 10f },
            { Stat.BaseHeavyAttackDamage, 30f }
        };

        _currentStats = new()
        {
            { Stat.Health, _baseStats[Stat.MaxHealth] / 2 },
            { Stat.Mana, _baseStats[Stat.MaxMana] },
            { Stat.HealthRegen, 1f },
            { Stat.ManaRegen, 1f },
            { Stat.CriticalChance, 0f },
            { Stat.CriticalMultiplier, 2f },
            { Stat.DamageReduction, 0f },
            { Stat.EvasionChance, 0f },
            { Stat.AttackSpeedModifier, 1f },
            { Stat.MovementSpeedModifier, 1f },
            { Stat.AttackDamageModifier, 1f },
            { Stat.LightAttackDamageModifier, 1f },
            { Stat.HeavyAttackDamageModifier, 1f },
        };

        Dictionary<Stat, float> totalStats = new()
        {
            { Stat.MovementSpeed, RecalculateTotal(Stat.MovementSpeed) },
            { Stat.AttackSpeed, RecalculateTotal(Stat.AttackSpeed) },
            { Stat.LightAttackDamage, RecalculateTotal(Stat.LightAttackDamage) },
            { Stat.HeavyAttackDamage, RecalculateTotal(Stat.HeavyAttackDamage) }
        };

        _currentStats.AddRange(totalStats);
    }

    public void IncrementStat(Stat stat, float amount)
    {
        if (_currentStats.ContainsKey(stat))
        {
            _currentStats[stat] += amount;

            ClampStat(stat);

            if (stat == Stat.MovementSpeedModifier)
            {
                SetCurrentStat(Stat.MovementSpeed, RecalculateTotal(Stat.MovementSpeed));
            }

            if (stat == Stat.AttackSpeedModifier)
            {
                SetCurrentStat(Stat.AttackSpeed, RecalculateTotal(Stat.AttackSpeed));
            }
            
            if (stat == Stat.AttackDamageModifier || stat == Stat.LightAttackDamageModifier)
            {
                SetCurrentStat(Stat.LightAttackDamage, RecalculateTotal(Stat.LightAttackDamage));
            }
            
            if (stat == Stat.AttackDamageModifier || stat == Stat.HeavyAttackDamageModifier)
            {
                SetCurrentStat(Stat.HeavyAttackDamage, RecalculateTotal(Stat.HeavyAttackDamage));
            }

            OnStatChanged(stat, _currentStats[stat]);
        }
        else
        {
            Debug.LogWarning($"Stat {stat} not found in dictionary.");
        }
    }

    public void SetCurrentStat(Stat stat, float value)
    {
        value = Mathf.Round(value * 10f) / 10f;

        if (_currentStats.ContainsKey(stat))
        {
            _currentStats[stat] = value;
            OnStatChanged(stat, value);
        }
        else
        {
            Debug.LogWarning($"Stat {stat} not found in dictionary.");
        }
    }

    public float GetCurrentStat(Stat stat)
    {
        if (_currentStats.ContainsKey(stat))
        {
            return _currentStats[stat];
        }
        Debug.LogWarning($"Stat {stat} not found in dictionary.");
        return 0f;
    }

    public float GetBaseStat(Stat stat)
    {
        if (_baseStats.ContainsKey(stat))
        {
            return _baseStats[stat];
        }
        Debug.LogWarning($"Stat {stat} not found in dictionary.");
        return 0f;
    }

    private void ClampStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.Health:
                float clampedHealth = Mathf.Clamp(GetCurrentStat(Stat.Health), 0f, GetBaseStat(Stat.MaxHealth));
                SetCurrentStat(Stat.Health, clampedHealth);
                break;

            case Stat.Mana:
                float clampedMana = Mathf.Clamp(GetCurrentStat(Stat.Mana), 0f, GetBaseStat(Stat.MaxMana));
                SetCurrentStat(Stat.Mana, clampedMana);
                break;

            case Stat.CriticalChance:
                float clampedCriticalChance = Mathf.Clamp(GetCurrentStat(Stat.CriticalChance), 0f, 100f);
                SetCurrentStat(Stat.CriticalChance, clampedCriticalChance);
                break;

            case Stat.EvasionChance:
                float clampedEvasionChance = Mathf.Clamp(GetCurrentStat(Stat.EvasionChance), 0f, 100f);
                SetCurrentStat(Stat.EvasionChance, clampedEvasionChance);
                break;

            case Stat.DamageReduction:
                float clampedDamageReduction = Mathf.Clamp(GetCurrentStat(Stat.DamageReduction), 0f, 100f);
                SetCurrentStat(Stat.DamageReduction, clampedDamageReduction);
                break;
        }
    }

    private float RecalculateTotal(Stat stat)
    {
        float totalStatValue = 0f;

        if (stat == Stat.MovementSpeed)
        {
            float baseMovementSpeed = _baseStats[Stat.BaseMovementSpeed];
            float movementModifier = _currentStats[Stat.MovementSpeedModifier];

            totalStatValue = baseMovementSpeed * movementModifier;
        }
        else if (stat == Stat.AttackSpeed)
        {
            float baseAttackSpeed = _baseStats[Stat.AttackSpeed];
            float attackSpeedModifier = _currentStats[Stat.AttackSpeedModifier];

            totalStatValue = baseAttackSpeed * attackSpeedModifier;
        }
        else if (stat == Stat.LightAttackDamage)
        {
            float baseDamage = _baseStats[Stat.BaseLightAttackDamage];
            float attackModifier = baseDamage * _currentStats[Stat.AttackDamageModifier] - baseDamage;
            float lightModifier = baseDamage * _currentStats[Stat.LightAttackDamageModifier] - baseDamage;

            totalStatValue = baseDamage + attackModifier + lightModifier;
        }
        else if (stat == Stat.HeavyAttackDamage)
        {
            float baseDamage = _baseStats[Stat.BaseHeavyAttackDamage];
            float attackModifier = baseDamage * _currentStats[Stat.AttackDamageModifier] - baseDamage;
            float heavyModifier = baseDamage * _currentStats[Stat.HeavyAttackDamageModifier] - baseDamage;

            totalStatValue = baseDamage + attackModifier + heavyModifier;
        }
        else
        {
            Debug.LogWarning("No total stat found to recalculate.");
        }

        return totalStatValue;
    }

    private void OnStatChanged(Stat stat, float value)
    {
        StatChanged?.Invoke(stat, value);
    }
}
