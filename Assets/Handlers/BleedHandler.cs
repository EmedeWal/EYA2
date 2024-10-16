using System.Collections;
using UnityEngine;
using System;

public class BleedHandler : MonoBehaviour
{
    [Header("VISUALIZATION")]
    [SerializeField] private VFX _bleedVFX;
    private VFXEmission _bleedEmission;
    private VFX _currentBleedVFX;

    private Coroutine _bleedCoroutine;
    private Transform _center;
    private Health _health;

    private BleedingStats _currentBleedingStats; 
    private int _currentStacks;

    public int CurrentStacks => _currentStacks;

    private VFXManager _VFXManager;

    public event Action<BleedHandler> BleedFinished;

    private void Start()
    {
        _center = GetComponent<LockTarget>().Center;
        _health = GetComponent<Health>();

        _VFXManager = VFXManager.Instance;
    }

    public void ApplyBleed(BleedingStats bleedingStats, int stackIncrement = 1)
    {
        _currentBleedingStats = bleedingStats;

        if (_bleedCoroutine != null)
        {
            _currentStacks = Mathf.Min(_currentStacks + stackIncrement, _currentBleedingStats.MaxStacks);
        }
        else
        {
            _currentBleedVFX = Instantiate(_bleedVFX, _center.position, _center.rotation);
            _bleedEmission = _currentBleedVFX.GetComponent<VFXEmission>();
            _bleedEmission.Init((float)_currentStacks / _currentBleedingStats.MaxStacks * 10);
            _VFXManager.AddMovingVFX(_currentBleedVFX, _center);

            _currentStacks = 1;
            _bleedCoroutine = StartCoroutine(HandleBleed());
        }
    }

    private void ResetBleed()
    {
        _VFXManager.RemoveVFX(_currentBleedVFX);
        _currentBleedVFX = null;
        _bleedCoroutine = null;
        _bleedEmission = null;
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
            _bleedEmission.Tick(emissionRate);

            duration--;
        }

        OnBleedFinished();
        ResetBleed();
    }

    private void OnDestroy()
    {
        if (_bleedCoroutine != null)
        {
            ResetBleed();
        }
    }
}
