using System.Collections;
using UnityEngine;
using System;

public abstract class Resource : MonoBehaviour
{
    protected float _Delta;

    private float _maxValue;
    private float _currentValue;
    private float _pendingValueChange;

    public event Action<GameObject> ValueExhausted;
    public event Action<float> MaxValueInitialized;
    public event Action<float> CurrentValueUpdated;
    public event Action<float> ValueRemoved;

    private Coroutine _decrementCoroutine;

    public float MaxValue => _maxValue;
    public float CurrentValue => _currentValue;
    public float RestorationModifier { private get; set; } = 1;

    public bool AtMinValue() => _currentValue <= 0;
    public bool AtMaxValue() => _currentValue >= _maxValue;

    public virtual void Init(float maxValue, float currentValue)
    {
        _maxValue = maxValue;
        _currentValue = currentValue;
        _pendingValueChange = 0;

        OnMaxValueInitialized(_maxValue);
        OnCurrentValueUpdated(_currentValue);
    }

    public virtual void LateTick(float delta)
    {
        _Delta = delta;

        if (_pendingValueChange != 0)
        {
            _currentValue += _pendingValueChange;

            if (_pendingValueChange < 0)
            {
                OnValueRemoved(_pendingValueChange);
            }

            if (_currentValue > _maxValue)
            {
                _currentValue = _maxValue;
            }
            else if (_currentValue <= 0)
            {
                _currentValue = 0;
                StopRemoveValueCoroutine();
                OnValueExhausted(gameObject);
            }

            _pendingValueChange = 0;
            OnCurrentValueUpdated(_currentValue);
        }
    }

    public void AddConstantValue(float value)
    {
        StartCoroutine(ModifyConstantValueCoroutine(value, true));
    }

    public void RemoveConstantValue(float value)
    {
        _decrementCoroutine = StartCoroutine(ModifyConstantValueCoroutine(value, false));
    }

    public void StopRemoveValueCoroutine()
    {
        if (_decrementCoroutine != null)
        {
            StopCoroutine(_decrementCoroutine);
        }
    }

    protected void AddValue(float amount)
    {
        _pendingValueChange += amount;
    }

    protected void RemoveValue(float amount)
    {
        _pendingValueChange -= amount;
    }

    protected virtual void OnValueRemoved(float amount)
    {
        ValueRemoved?.Invoke(amount);
    }

    private void OnValueExhausted(GameObject exhaustedObject)
    {
        ValueExhausted?.Invoke(exhaustedObject);
    }

    private void OnMaxValueInitialized(float maxValue)
    {
        MaxValueInitialized?.Invoke(maxValue);
    }

    private void OnCurrentValueUpdated(float currentValue)
    {
        CurrentValueUpdated?.Invoke(currentValue);
    }

    private IEnumerator ModifyConstantValueCoroutine(float value, bool increment)
    {
        while (true)
        {
            float deltaValue = value * _Delta;

            if (increment)
            {
                AddValue(deltaValue * RestorationModifier);
            }
            else
            {
                RemoveValue(deltaValue);
            }

            yield return null;
        }
    }
}