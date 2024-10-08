using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mastery Perk", menuName = "Scriptable Object/Perks/Passive Perk/Mastery")]
public class MasteryPerk : PerkData
{
    private PlayerAttackHandler _playerAttackHandler;

    [Header("BLADE DANCE STATS")]
    [SerializeField] private float _lightAttackBoostDuration = 6f;
    [SerializeField] private float _heavyAttackBoostDuration = 3f;
    [SerializeField] private float _attackDamageBoostMax = 0.5f;
    [SerializeField] private float _attackDamageBoost = 0.1f;
    private float _lightAttackTimer;
    private float _heavyAttackTimer;

    [Header("CRESCENDO BRANCH")]
    [SerializeField] private VFX _bloodwaveVFX;

    [Header("FLOW BRANCH")]
    [SerializeField] private VFX _flowVFX;
    [SerializeField] private int _maxFlow = 5;
    [SerializeField] private float _flowDuration = 5f;
    [SerializeField] private float _maxCriticalChanceBoost = 30f;
    [SerializeField] private float _maxCriticalMultiplierBoost = 0.5f;
    private VFXEmission _emission;
    private VFX _currentFlowVFX;
    private AttackType _previousAttack;
    private int _currentFlow;
    private float _flowTimer;
    private float _criticalChanceBoost;
    private float _criticalMultiplierBoost;

    private Dictionary<Stat, float> _statChanges = new();
    private LayerMask _targetLayer;

    private VFXManager _VFXManager;

    public override void Init(GameObject playerObject, List<PerkData> perks = null)
    {
        base.Init(playerObject, perks);

        _playerAttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();

        _criticalChanceBoost = _maxCriticalChanceBoost / _maxFlow;
        _criticalMultiplierBoost = _maxCriticalMultiplierBoost / _maxFlow;

        _targetLayer = LayerMask.GetMask("DamageCollider");

        _VFXManager = VFXManager.Instance;
    }

    public override void Activate()
    {
        _playerAttackHandler.SuccessfulHit += MasteryPerk_SuccesfulHit;
        _playerAttackHandler.LightAttack += MasteryPerk_LightAttack;
        _playerAttackHandler.HeavyAttack += MasteryPerk_HeavyAttack;

        _statChanges.Add(Stat.LightAttackDamageModifier, 0);
        _statChanges.Add(Stat.HeavyAttackDamageModifier, 0);
        _statChanges.Add(Stat.CriticalChance, 0);
        _statChanges.Add(Stat.CriticalMultiplier, 0);

        _lightAttackTimer = 0;
        _heavyAttackTimer = 0;

        _currentFlow = 0;
        _flowTimer = 0;

        if (_flowVFX != null)
        {
            _currentFlowVFX = Instantiate(_flowVFX, _PlayerTransform);
            _emission = _currentFlowVFX.GetComponent<VFXEmission>();
            _VFXManager.AddVFX(_currentFlowVFX, _PlayerTransform);
            _emission.Init(_currentFlow);
        }
    }

    public override void Deactivate()
    {
        _playerAttackHandler.SuccessfulHit -= MasteryPerk_SuccesfulHit;
        _playerAttackHandler.LightAttack -= MasteryPerk_LightAttack;
        _playerAttackHandler.HeavyAttack -= MasteryPerk_HeavyAttack;

        ResetStatChanges();
        _statChanges.Clear();

        if (_currentFlowVFX != null)
        {
            _VFXManager.RemoveVFX(_currentFlowVFX);
            _currentFlowVFX = null;
            _emission = null;
        }
    }

    public override void Tick(float delta)
    {
        _lightAttackTimer += delta;
        _heavyAttackTimer += delta;
        _flowTimer += delta;

        if (_statChanges[Stat.LightAttackDamageModifier] > 0 && _lightAttackTimer >= _lightAttackBoostDuration)
        {
            IncrementStat(Stat.LightAttackDamageModifier, -_statChanges[Stat.LightAttackDamageModifier]);
            _lightAttackTimer = 0;
        }
        
        if (_statChanges[Stat.HeavyAttackDamageModifier] > 0 && _heavyAttackTimer >= _heavyAttackBoostDuration)
        {
            IncrementStat(Stat.HeavyAttackDamageModifier, -_statChanges[Stat.HeavyAttackDamageModifier]);
            _heavyAttackTimer = 0;
        }

        if (_flowTimer >= _flowDuration)
        {
            ResetFlow();
        }
    }

    private void MasteryPerk_SuccesfulHit(Collider hit, float damage, bool crit)
    {

    }

    private void MasteryPerk_LightAttack()
    {
        if (_currentFlowVFX != null)
        {
            HandleFlow(AttackType.Light);
        }

        _previousAttack = AttackType.Light;
        _heavyAttackTimer = 0;

        if (_statChanges[Stat.HeavyAttackDamageModifier] < _attackDamageBoostMax)
        {
            IncrementStat(Stat.HeavyAttackDamageModifier, _attackDamageBoost);
        }
    }

    private void MasteryPerk_HeavyAttack()
    {
        if (_currentFlowVFX != null)
        {
            HandleFlow(AttackType.Heavy);
        }

        _previousAttack = AttackType.Heavy;
        _lightAttackTimer = 0;

        if (_statChanges[Stat.LightAttackDamageModifier] < _attackDamageBoostMax)
        {
            IncrementStat(Stat.LightAttackDamageModifier, _attackDamageBoost);
        }       
    }

    private void HandleFlow(AttackType attackType)
    {
        if (_currentFlow >= _maxFlow) return;

        if (_previousAttack != attackType)
        {
            IncreaseFlow();
        }
        else
        {
            ResetFlow();
        }
    }

    private void IncreaseFlow()
    {
        _flowTimer = 0;
        _currentFlow++;
        _emission.Tick(_currentFlow * 5f);
        IncrementStat(Stat.CriticalChance, _criticalChanceBoost);
        IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierBoost);
    }

    private void ResetFlow()
    {
        _flowTimer = 0;
        _currentFlow = 0;
        _emission.Tick(_currentFlow * 5f);
        IncrementStat(Stat.CriticalChance, -_statChanges[Stat.CriticalChance]);
        IncrementStat(Stat.CriticalMultiplier, -_statChanges[Stat.CriticalMultiplier]);
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