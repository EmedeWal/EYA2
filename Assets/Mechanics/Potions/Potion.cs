using System.Collections;
using UnityEngine;
using System;

public abstract class Potion : MonoBehaviour
{
    [Header("CHARGES")]
    [SerializeField] protected int _MaxCharges = 2;
    protected int _CurrentCharges;

    [Header("REFILLS")]
    [SerializeField] private float _refillDuration = 10f;
    [SerializeField] private float _refillOnKill = 5f;
    private float _remainingTime;

    [Header("EFFECT STATISTICS")]
    [SerializeField] protected float _RefillAmount = 25f;
    [SerializeField] protected float _RefillTime = 2.5f;
    [SerializeField] protected float _RefillSpeed = 10f;
    private bool _isRefilling = false;

    [Header("VISUAL FEEDBACK")]
    [SerializeField] private GameObject _potionVisuals;
    [SerializeField] private Transform _VFXOrigin;

    [Header("AUDIO")]
    [SerializeField] private AudioSource _audioSource;

    public event Action<int> ChargesUpdated;
    public event Action<float> RefillStarted;
    public event Action<float> RefillUpdated;

    public void ConsumePotion()
    {
        if (!_audioSource.isPlaying) _audioSource.Play();

        SetCharges(_CurrentCharges - 1);
        TriggerPotionEffect();
        StartRefill();
    }

    protected virtual void TriggerPotionEffect()
    {
        GameObject visuals = Instantiate(_potionVisuals, _VFXOrigin);
        visuals.GetComponent<Lifetime>().DestroyAfterDelay(_RefillTime);
    }

    protected void SetCharges(int newCharges)
    {
        _CurrentCharges = newCharges;
        OnChargesUpdated();
    }

    //private void Potion_EnemyDied()
    //{
    //    BoostRefill();
    //}

    private void BoostRefill()
    {
        if (!_isRefilling) return;

        _isRefilling = false;
        _remainingTime -= _refillOnKill;

        if (_remainingTime > 0) return;

        RefillCharge();
        StopAllCoroutines();

        if (AtMaxCharges()) return;

        _remainingTime *= -1;
        StartCoroutine(RefillCoroutine(_refillDuration - _remainingTime));
    }

    private void StartRefill()
    {
        if (_isRefilling) return;

        StartCoroutine(RefillCoroutine(_refillDuration));
    }

    private void RefillCharge()
    {
        if (AtMaxCharges()) return;

        SetCharges(_CurrentCharges + 1);
        OnRefillUpdated(0);
    }

    private IEnumerator RefillCoroutine(float duration)
    {
        _isRefilling = true;
        _remainingTime = duration;
        OnRefillStarted(duration);

        while (_remainingTime > 0)
        {
            yield return null;
            _remainingTime -= Time.deltaTime;
            OnRefillUpdated(_remainingTime);
        }

        _isRefilling = false;

        RefillCharge();
        if (!AtMaxCharges()) StartRefill();
    }

    private bool AtMaxCharges()
    {
        return _CurrentCharges == _MaxCharges;
    }

    private void OnChargesUpdated()
    {
        ChargesUpdated?.Invoke(_CurrentCharges);
    }

    private void OnRefillStarted(float time)
    {
        RefillStarted?.Invoke(time);
    }

    private void OnRefillUpdated(float time)
    {
        RefillUpdated?.Invoke(time);
    }
}