using UnityEngine;

public class BattleZone : MonoBehaviour
{
    private PlayerStatTracker _statTracker;
 
    private float _criticalChanceBonus = 0;
    private float _criticalMultiplierBonus = 0;
    private float _damageReductionBonus = 0;
    private float _healthRegenBonus = 0;

    private int _enemyCounter = 0;

    public bool PlayerInside { get; private set; } = false;

    public void Init(PlayerStatTracker statTracker, float criticalChanceBonus, float criticalMultiplierBonus, float damageReductionBonus, float healthRegenBonus)
    {
        _statTracker = statTracker;


        _criticalChanceBonus = criticalChanceBonus;
        _criticalMultiplierBonus = criticalMultiplierBonus;
        _damageReductionBonus = damageReductionBonus;
        _healthRegenBonus = healthRegenBonus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInside = true;

            _statTracker.IncrementStat(Stat.CriticalChance, _criticalChanceBonus);
            _statTracker.IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierBonus);
            _statTracker.IncrementStat(Stat.DamageReduction, _damageReductionBonus * _enemyCounter);
            _statTracker.IncrementStat(Stat.HealthRegen, _healthRegenBonus * _enemyCounter);
        }

        if (other.CompareTag("Enemy"))
        {
            _enemyCounter++;

            if (PlayerInside)
            {
                _statTracker.IncrementStat(Stat.DamageReduction, _damageReductionBonus);
                _statTracker.IncrementStat(Stat.HealthRegen, _healthRegenBonus);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInside = false;

            _statTracker.ResetStatChanges();
        }

        if (other.CompareTag("Enemy"))
        {
            _enemyCounter--;

            if (PlayerInside)
            {
                _statTracker.IncrementStat(Stat.DamageReduction, -_damageReductionBonus);
                _statTracker.IncrementStat(Stat.HealthRegen, -_healthRegenBonus);
            }
        }
    }
}
