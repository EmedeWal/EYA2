using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vampire Ultimate", menuName = "Scriptable Object/Perks/Ultimate Perk/Vampire")]
public class VampireUltimate : PerkData
{ 
    private Dictionary<Stat, float> _statChanges = new();

    [Header("FEAST STATS")]
    [SerializeField] private VFX _feastVFX;
    [SerializeField] private float _healthGainPercentage;
    [SerializeField] private float _manaGainPercentage;

    [Header("BLOOD REAPER STATS")]
    [SerializeField] private float _attackDamageGain = 0.01f;
    [SerializeField] private float _criticalChanceGain = 0.1f;
    [SerializeField] private float _criticalMultiplierGain = 0.05f;

    [Header("ONSLAUGHT STATS")]
    [SerializeField] private BloodwaveStats _bloodwaveStats;
    [SerializeField] private VFX _bloodwaveVFX;
    [SerializeField] private bool _guaranteeCritical = true;

    public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
    {
        base.Init(playerObject, perks, statChanges);

        if (_bloodwaveStats != null)
        {
            _bloodwaveStats.ActivateMultiplier();
        }
    }

    public override void Activate()
    {
        base.Activate();

        CreatureManager.CreatureDeath += VampireUltimate_CreatureDeath;
        _AttackHandler.SuccessfulAttack += VampireUltimate_SuccessfulAttack;

        _statChanges.Add(Stat.AttackDamageModifier, 0);
        _statChanges.Add(Stat.CriticalChance, 0);
        _statChanges.Add(Stat.CriticalMultiplier, 0);

    }

    public override void Deactivate()
    {
        base.Deactivate();

        CreatureManager.CreatureDeath -= VampireUltimate_CreatureDeath;
        _AttackHandler.SuccessfulAttack -= VampireUltimate_SuccessfulAttack;

        ResetStatChanges();
        _statChanges.Clear();
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
    }

    private void VampireUltimate_CreatureDeath(CreatureAI creature)
    {
        _VFXManager.AddMovingVFX(_feastVFX, _PlayerTransform, 1f);

        _Health.Heal(_PlayerStats.GetBaseStat(Stat.MaxHealth) / 100 * _healthGainPercentage);
        _Mana.Gain(_PlayerStats.GetBaseStat(Stat.MaxMana) / 100 * _manaGainPercentage);   

        if (creature.TryGetComponent(out BleedHandler bleedHandler))
        {
            int stacks = bleedHandler.CurrentStacks;
            IncrementStat(Stat.AttackDamageModifier, _attackDamageGain * stacks);
            IncrementStat(Stat.CriticalChance, _criticalChanceGain * stacks);
            IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierGain * stacks);
        }

        if (_guaranteeCritical)
        {
            _AttackHandler.GuaranteedCrit = true;
        }
    }

    private void VampireUltimate_SuccessfulAttack(Collider hit, int colliders, float damage, bool crit)
    {
        if (crit && _bloodwaveVFX != null)
        {
            _VFXManager.AddStaticVFX(_bloodwaveVFX, _PlayerTransform.position, _PlayerTransform.rotation, 3f);
        }
    }

    private void IncrementStat(Stat stat, float value)
    {
        _statChanges[stat] += value;
        _PlayerStats.IncrementStat(stat, value);
    }

    private void ResetStatChanges()
    {
        var statChangeCopy = new Dictionary<Stat, float>(_statChanges);

        foreach (var statChange in statChangeCopy)
        {
            if (statChange.Value != 0)
            {
                _PlayerStats.IncrementStat(statChange.Key, -statChange.Value);
                _statChanges[statChange.Key] = 0;
            }
        }
    }
}
