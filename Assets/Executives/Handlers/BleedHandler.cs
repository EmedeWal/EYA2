using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BleedHandler : MonoBehaviour
{
    [Header("VISUALIZATION")]
    [SerializeField] private VFX _bleedVFX;
    private VFXEmission _bleedVFXEmission;
    private VFX _currentBleedVFX;
    private float _emissionRate;

    private BleedingStats _currentBleedingStats;
    private int _currentStacks;

    private CreatureStatTracker _statTracker;
    private Coroutine _coroutine;
    private Transform _center;
    private Health _health;

    private VFXManager _VFXManager;

    public int CurrentStacks => _currentStacks;

    public event Action<BleedHandler> BleedFinished;

    public void Init()
    {
        Dictionary<Stat, float> statChanges = new()
        {
            { Stat.DamageReduction, 0 },
            { Stat.AttackDamageModifier, 0 }
        };

        _statTracker = new CreatureStatTracker (statChanges, GetComponent<CreatureStatManager>());
        _center = GetComponent<LockTarget>().Center;
        _health = GetComponent<Health>();

        _VFXManager = VFXManager.Instance;
    }

    public void Cleanup()
    {
        ResetBleed();
    }

    public void Tick()
    {
        if (_bleedVFXEmission != null)
        {
            _bleedVFXEmission.Tick(_emissionRate);
        }
    }

    public void ApplyBleed(BleedingStats bleedingStats, int stackIncrement = 1)
    {
        _currentBleedingStats = bleedingStats;

        if (_coroutine != null)
        {
            _currentStacks = Mathf.Min(_currentStacks + stackIncrement, _currentBleedingStats.MaxStacks);
            _emissionRate = (float)_currentStacks / _currentBleedingStats.MaxStacks * 10;

            StopCoroutine(_coroutine);
            HandleBleedNerfs(stackIncrement);
            _coroutine = StartCoroutine(HandleBleed());
        }
        else
        {
            _emissionRate = (float)_currentStacks / _currentBleedingStats.MaxStacks * 10;
            _currentBleedVFX = _VFXManager.AddMovingVFX(_bleedVFX, _center);
            _bleedVFXEmission = _currentBleedVFX.GetComponent<VFXEmission>();
            _bleedVFXEmission.Init(_emissionRate);

            _currentStacks = stackIncrement;
            _coroutine = StartCoroutine(HandleBleed());
        }
    }

    public void ResetBleed()
    {
       if (_coroutine != null )
        {
            StopCoroutine(_coroutine);
        }

        _VFXManager.RemoveVFX(_currentBleedVFX, 1f);
        _statTracker.ResetStatChanges();
        _bleedVFXEmission = null;
        _currentBleedVFX = null;
        _coroutine = null;

        OnBleedFinished();
    }

    private void HandleBleedNerfs(int stacks)
    {
        float damageReductionDecrement = -(stacks * _currentBleedingStats.DamageInflictedModifier);
        float damageInflictionDecrement = -(stacks * _currentBleedingStats.DamageInflictedModifier);

        _statTracker.IncrementStat(Stat.DamageReduction, damageReductionDecrement);
        _statTracker.IncrementStat(Stat.AttackDamageModifier, damageInflictionDecrement);
    }

    private void OnBleedFinished()
    {
        BleedFinished?.Invoke(this);
    }

    private IEnumerator HandleBleed()
    {
        float duration = _currentBleedingStats.Duration;

        while (duration > 0)
        {
            yield return new WaitForSeconds(1f);

            if (_health == null) yield break;

            float totalDamage = _currentBleedingStats.Damage * _currentStacks;
            _health.TakeDamage(null, totalDamage);

            float emissionRate = (float)_currentStacks / _currentBleedingStats.MaxStacks * 10;
            _bleedVFXEmission.Tick(emissionRate);

            duration--;
        }

        ResetBleed();
    }
}