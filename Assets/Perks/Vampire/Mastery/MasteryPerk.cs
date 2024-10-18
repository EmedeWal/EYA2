using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Mastery Perk", menuName = "Scriptable Object/Perks/Passive Perk/Mastery")]
public class MasteryPerk : PerkData
{
    private List<float> _timerList;

    [Header("VARIABLES")]
    [SerializeField] private float _maxIdleDuration = 3f;
    private AttackType _previousAttack;

    [Header("BLADE DANCE STATS")]
    [SerializeField] private float _lightAttackBoostDuration = 6f;
    [SerializeField] private float _heavyAttackBoostDuration = 3f;
    [SerializeField] private float _attackDamageBoostMax = 0.5f;
    [SerializeField] private float _attackDamageBoost = 0.1f;

    [Header("CRESCENDO BRANCH")]
    [SerializeField] private List<ComboData> _comboDataList;

    [Header("FLOW BRANCH")]
    [SerializeField] private VFX _flowVFX;
    [SerializeField] private int _maxFlow = 5;
    [SerializeField] private float _maxCriticalChanceBoost = 30f;
    [SerializeField] private float _maxCriticalMultiplierBoost = 0.5f;
    private VFXEmission _emission;
    private VFX _currentFlowVFX;
    private int _currentFlow;
    private float _criticalChanceBoost;
    private float _criticalMultiplierBoost;

    public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
    {
        statChanges = new()
        {
            { Stat.LightAttackDamageModifier, 0 },
            { Stat.HeavyAttackDamageModifier, 0 },
            { Stat.CriticalChance, 0 },
            { Stat.CriticalMultiplier, 0 }
        };

        base.Init(playerObject, perks, statChanges);

        _timerList = new() { 0, 0, 0 };

        foreach (var comboData in _comboDataList)
        {
            comboData.Init(_VFXManager, _PlayerTransform);
            comboData.ComboFinished += MasteryPerk_ComboFinished;
        }

        _criticalChanceBoost = _maxCriticalChanceBoost / _maxFlow;
        _criticalMultiplierBoost = _maxCriticalMultiplierBoost / _maxFlow;
    }

    public override void Activate()
    {
        _AttackHandler.AttackBegun += MasteryPerk_AttackBegun;
        _AttackHandler.AttackEnded += MasteryPerk_AttackEnded;

        _previousAttack = AttackType.None;

        for (int i = 0; i < _timerList.Count; i++)
        {
            _timerList[i] = 0;
        }

        _currentFlow = 0;

        if (_flowVFX != null)
        {
            _currentFlowVFX = _VFXManager.AddMovingVFX(_flowVFX, _PlayerTransform);
            _emission = _currentFlowVFX.GetComponent<VFXEmission>();
            _emission.Init(_currentFlow);
        }
    }

    public override void Deactivate()
    {
        _AttackHandler.AttackBegun -= MasteryPerk_AttackBegun;
        _AttackHandler.AttackEnded -= MasteryPerk_AttackEnded;

        _StatTracker.ResetStatChanges();

        if (_currentFlowVFX != null)
        {
            _VFXManager.RemoveVFX(_currentFlowVFX);
            _currentFlowVFX = null;
            _emission = null;
        }
    }

    public override void Tick(float delta)
    {
        foreach (var comboData in _comboDataList)
        {
            comboData.Tick(delta);
        }

        if (_AttackHandler.IsAttacking)
        {
            for (int i = 0; i < _timerList.Count; i++)
            {
                _timerList[i] = 0;
            }
        }
        else
        {
            for (int i = 0; i < _timerList.Count; i++)
            {
                _timerList[i] += delta;
            }

            if (_timerList[0] >= _maxIdleDuration)
            {
                foreach (var comboData in _comboDataList)
                {
                    comboData.ResetCombo();
                }

                if (_flowVFX != null)
                {
                    ResetFlow();
                }
            }
        }

        if (_timerList[1] >= _lightAttackBoostDuration)
        {
            _timerList[1] = HandleIdle(Stat.LightAttackDamageModifier, -_StatTracker.GetStatChange(Stat.LightAttackDamageModifier), _timerList[1]);
        }

        if (_timerList[2] >= _heavyAttackBoostDuration)
        {
            _timerList[2] = HandleIdle(Stat.HeavyAttackDamageModifier, -_StatTracker.GetStatChange(Stat.HeavyAttackDamageModifier), _timerList[2]);
        }
    }

    private void MasteryPerk_AttackBegun(AttackType attackType)
    {
        foreach (var comboData in _comboDataList)
        {
            comboData.RegisterAttackStarted(attackType);
        }
    }

    private void MasteryPerk_AttackEnded(AttackType attackType)
    {
        foreach (var comboData in _comboDataList)
        {
            comboData.RegisterAttackFinished(attackType);
        }

        if (_currentFlowVFX != null && _previousAttack != AttackType.None)
        {
            HandleFlow(attackType);
        }

        _previousAttack = attackType;

        if (attackType == AttackType.Light)
        {
            _timerList[2] = HandleAttack(Stat.HeavyAttackDamageModifier);
        }
        else if (attackType == AttackType.Heavy)
        {
            _timerList[1] = HandleAttack(Stat.LightAttackDamageModifier);
        }
    }

    private void MasteryPerk_ComboFinished()
    {
        GameManager.Instance.StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return new WaitForEndOfFrame();
            foreach (var comboData in _comboDataList)
            {
                comboData.ResetCombo();
            }
        }
    }

    private void HandleFlow(AttackType attackType)
    {
        if (_previousAttack != attackType)
        {
            if (_currentFlow < _maxFlow)
            {
                IncreaseFlow();
            }
        }
        else
        {
            ResetFlow();
        }
    }

    private void IncreaseFlow()
    {
        _currentFlow++;
        _emission.Tick(_currentFlow * 5f);
        _StatTracker.IncrementStat(Stat.CriticalChance, _criticalChanceBoost);
        _StatTracker.IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierBoost);
    }

    private void ResetFlow()
    {
        _currentFlow = 0;
        _emission.Tick(_currentFlow * 5f);
        _StatTracker.IncrementStat(Stat.CriticalChance, -_StatTracker.GetStatChange(Stat.CriticalChance));
        _StatTracker.IncrementStat(Stat.CriticalMultiplier, -_StatTracker.GetStatChange(Stat.CriticalMultiplier));
    }

    private float HandleIdle(Stat stat, float value, float timer)
    {
        if (_StatTracker.GetStatChange(stat) > 0)
        {
            _StatTracker.IncrementStat(stat, value);
        }
        else
        {
            return timer;
        }
        return 0;
    }

    private float HandleAttack(Stat stat)
    {
        if (_StatTracker.GetStatChange(stat) < _attackDamageBoostMax)
        {
            _StatTracker.IncrementStat(stat, _attackDamageBoost);
        }

        return 0;
    }
}