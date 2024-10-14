using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Mastery Perk", menuName = "Scriptable Object/Perks/Passive Perk/Mastery")]
public class MasteryPerk : PerkData
{
    private PlayerAttackHandler _playerAttackHandler;
    private VFXManager _VFXManager;

    private Dictionary<Stat, float> _statChanges;
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

    public override void Init(GameObject playerObject, List<PerkData> perks = null)
    {
        base.Init(playerObject, perks);

        for (int i = perks.Count - 1; i >= 0; i--)
        {
            PerkData perk = perks[i];
            if (perk.GetType() == GetType())
            {
                perk.Deactivate();
                perks.RemoveAt(i);
            }
        }

        _playerAttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();
        _VFXManager = VFXManager.Instance;

        _statChanges = new Dictionary<Stat, float>();
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
        _playerAttackHandler.AttackBegun += MasteryPerk_AttackBegun;
        _playerAttackHandler.AttackEnded += MasteryPerk_AttackEnded;

        _statChanges.Add(Stat.LightAttackDamageModifier, 0);
        _statChanges.Add(Stat.HeavyAttackDamageModifier, 0);
        _statChanges.Add(Stat.CriticalChance, 0);
        _statChanges.Add(Stat.CriticalMultiplier, 0);

        _previousAttack = AttackType.None;

        for (int i = 0; i < _timerList.Count; i++)
        {
            _timerList[i] = 0;
        }

        _currentFlow = 0;

        if (_flowVFX != null)
        {
            _currentFlowVFX = _VFXManager.AddVFX(_flowVFX, false, 0, _PlayerTransform.position, _PlayerTransform.rotation, _PlayerTransform);
            _emission = _currentFlowVFX.GetComponent<VFXEmission>();
            _emission.Init(_currentFlow);
        }
    }

    public override void Deactivate()
    {
        _playerAttackHandler.AttackBegun -= MasteryPerk_AttackBegun;
        _playerAttackHandler.AttackEnded -= MasteryPerk_AttackEnded;

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
        foreach (var comboData in _comboDataList)
        {
            comboData.Tick(delta);
        }

        if (_playerAttackHandler.IsAttacking)
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
            _timerList[1] = HandleIdle(Stat.LightAttackDamageModifier, -_statChanges[Stat.LightAttackDamageModifier], _timerList[1]);
        }

        if (_timerList[2] >= _heavyAttackBoostDuration)
        {
            _timerList[2] = HandleIdle(Stat.HeavyAttackDamageModifier, -_statChanges[Stat.HeavyAttackDamageModifier], _timerList[2]);
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
        IncrementStat(Stat.CriticalChance, _criticalChanceBoost);
        IncrementStat(Stat.CriticalMultiplier, _criticalMultiplierBoost);
    }

    private void ResetFlow()
    {
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

    private float HandleIdle(Stat stat, float value, float timer)
    {
        if (_statChanges[stat] > 0)
        {
            IncrementStat(stat, value);
        }
        else
        {
            return timer;
        }
        return 0;
    }

    private float HandleAttack(Stat stat)
    {
        if (_statChanges[stat] < _attackDamageBoostMax)
        {
            IncrementStat(stat, _attackDamageBoost);
        }

        return 0;
    }
}