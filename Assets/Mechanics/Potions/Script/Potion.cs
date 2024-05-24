using System;
using System.Collections;
using UnityEngine;

public abstract class Potion : MonoBehaviour
{
    [Header("CHARGES AND REFILLING")]
    [SerializeField] private int _maxCharges = 2;
    [SerializeField] private float _refillDuration = 10f;
    [SerializeField] private float _refillOnKill = 5f;
    private float _remainingTime;

    [Header("EFFECT")]
    [SerializeField] protected float RefillAmount = 25f;
    protected int CurrentCharges;
    private bool _isRefilling = false;

    [Header("AUDIO")]
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        EnemyHealth.EnemyDied += Potion_EnemyDied;
    }

    private void OnDestroy()
    {
        EnemyHealth.EnemyDied -= Potion_EnemyDied;
    }

    private void Potion_EnemyDied()
    {
        BoostRefill();
    }

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

    protected void SetCharges()
    {
        CurrentCharges = _maxCharges;

        ChargesChanged(CurrentCharges);
    }

    public void ConsumePotion()
    {
        if (!_audioSource.isPlaying) _audioSource.Play();

        CurrentCharges--;

        ChargesChanged(CurrentCharges);
        StartRefill();
    }

    private void StartRefill()
    {
        if (_isRefilling) return;

        StartCoroutine(RefillCoroutine(_refillDuration));
    }

    private IEnumerator RefillCoroutine(float duration)
    {
        RefillStarted(_refillDuration);

        _isRefilling = true;
        _remainingTime = duration;

        while (_remainingTime > 0)
        {
            yield return null;
            _remainingTime -= Time.deltaTime;

            RefillUpdated(_remainingTime);
        }

        _isRefilling = false;

        RefillCharge();
        if (!AtMaxCharges()) StartRefill();
    }

    private void RefillCharge()
    {
        if (AtMaxCharges()) return;

        CurrentCharges++;

        RefillUpdated(0);
        ChargesChanged(CurrentCharges);
    }

    private bool AtMaxCharges()
    {
        return CurrentCharges == _maxCharges;
    }

    protected abstract void ChargesChanged(int currentCharges);

    protected abstract void RefillStarted(float startTime);

    protected abstract void RefillUpdated(float remainingTime);
}