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
    [SerializeField] private float _refillAmount = 25f;
    protected int CurrentCharges;
    private bool _isRefilling = false;

    [Header("AUDIO")]
    [SerializeField] private AudioSource _audioSource;

    protected event Action<int> ChargesChanged;
    protected event Action<float> RefillStarted;
    protected event Action<float> RefillBoosted;
    protected event Action<float> PotionConsumed;

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
        if (_isRefilling)
        {
            _remainingTime -= _refillOnKill;

            OnRefillBoosted(_refillOnKill);
        }
    }

    protected void SetCharges()
    {
        CurrentCharges = _maxCharges;

        OnChargesChanged();
    }

    public void ConsumePotion()
    {
        CurrentCharges--;

        if (!_audioSource.isPlaying) _audioSource.Play();

        OnPotionConsumed();
        OnChargesChanged();

        StartCooldown();
    }

    public void StartCooldown()
    {
        if (!_isRefilling)
        {
            _isRefilling = true;

            OnRefillStarted(_refillDuration);
            StartCoroutine(RefillCoroutine(_refillDuration));
        }
    }

    private IEnumerator RefillCoroutine(float duration)
    {
        _remainingTime = duration;

        while (_remainingTime > 0)
        {
            yield return null;
            _remainingTime -= Time.deltaTime;
        }

        Refill();

        _isRefilling = false;

        if (CurrentCharges < _maxCharges)
        {
            StartCooldown();
        }
    }

    public void Refill()
    {
        if (CurrentCharges < _maxCharges)
        {
            CurrentCharges++;
            OnChargesChanged();
        }
    }

    private void OnPotionConsumed()
    {
        PotionConsumed?.Invoke(_refillAmount);
    }

    private void OnChargesChanged()
    {
        ChargesChanged?.Invoke(CurrentCharges);
    }

    private void OnRefillStarted(float duration)
    {
        RefillStarted?.Invoke(duration);
    }

    private void OnRefillBoosted(float boost)
    {
        RefillBoosted?.Invoke(boost);   
    }
}