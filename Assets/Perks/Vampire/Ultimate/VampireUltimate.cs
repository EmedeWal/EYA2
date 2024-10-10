using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vampire Ultimate", menuName = "Scriptable Object/Perks/Ultimate Perk/Vampire")]
public class VampireUltimate : PerkData
{ 
    private VFXManager _VFXManager;

    private PlayerAttackHandler _playerAttackHandler;   
    private Health _playerHealth;
    private Mana _playerMana;

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

    public override void Init(GameObject playerObject, List<PerkData> perks = null)
    {
        base.Init(playerObject, perks);

        _VFXManager = VFXManager.Instance;

        _playerAttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();
        _playerHealth = _PlayerObject.GetComponent<Health>();
        _playerMana = _PlayerObject.GetComponent<Mana>();

        if (_bloodwaveStats != null)
        {
            _bloodwaveStats.ActivateMultiplier();
        }
    }

    public override void Activate()
    {
        base.Activate();

        // Subscribe to static enemydeath
        _playerAttackHandler.SuccessfulAttack += VampireUltimate_SuccessfulAttack;

        _statChanges.Add(Stat.AttackDamageModifier, 0);
        _statChanges.Add(Stat.CriticalChance, 0);
        _statChanges.Add(Stat.CriticalMultiplier, 0);

    }

    public override void Deactivate()
    {
        base.Deactivate();

        // Unsubscribe to static enemydeath
        _playerAttackHandler.SuccessfulAttack -= VampireUltimate_SuccessfulAttack;

        ResetStatChanges();
        _statChanges.Clear();
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
    }

    private void VampireUltimate_CreatureDeath(CreatureAI creature)
    {
        _VFXManager.AddVFX(_feastVFX, true, 1f, _PlayerTransform.position, _PlayerTransform.rotation, _PlayerTransform);
        _playerHealth.Heal(_PlayerStats.GetBaseStat(Stat.MaxHealth) / 100 * _healthGainPercentage);
        _playerMana.Gain(_PlayerStats.GetBaseStat(Stat.MaxMana) / 100 * _manaGainPercentage);   

        if (creature.TryGetComponent(out BleedHandler bleedHandler))
        {
            int stacks = bleedHandler.CurrentStacks;
            IncrementStat(Stat.AttackDamageModifier, _attackDamageGain * stacks);
            IncrementStat(Stat.CriticalChance, _criticalChanceGain * stacks);
            IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierGain * stacks);
        }

        if (_guaranteeCritical)
        {
            _playerAttackHandler.GuaranteedCrit = true;
        }
    }

    private void VampireUltimate_SuccessfulAttack(Collider hit, int colliders, float damage, bool crit)
    {
        if (crit && _bloodwaveVFX != null)
        {
            _VFXManager.AddVFX(_bloodwaveVFX, true, 3f, _PlayerTransform.position, _PlayerTransform.rotation, _PlayerTransform);
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
