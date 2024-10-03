using System.Collections;
using UnityEngine;
using System;

public abstract class Resource : MonoBehaviour
{
    private float _delta;

    private float _maxValue;
    private float _currentValue;
    private float _pendingValueChange;

    public event Action<GameObject> ValueExhausted;
    public event Action<float> MaxValueInitialized;
    public event Action<float> CurrentValueUpdated;
    public event Action ValueRemoved;

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

        OnMaxValueInitialized();
        OnCurrentValueUpdated();
    }

    protected void AddValue(float amount)
    {
        _pendingValueChange += amount;
    }

    protected void RemoveValue(float amount)
    {
        _pendingValueChange -= amount;
    }

    public void LateTick(float delta)
    {
        _delta = delta;

        if (_pendingValueChange != 0)
        {
            _currentValue += _pendingValueChange;

            if (_pendingValueChange < 0)
            {
                OnValueRemoved();
            }

            if (_currentValue > _maxValue)
            {
                _currentValue = _maxValue;
            }
            else if (_currentValue < 0)
            {
                _currentValue = 0;
                StopRemoveValueCoroutine();
                OnValueExhausted(gameObject);
            }

            _pendingValueChange = 0;
            OnCurrentValueUpdated();
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

    private IEnumerator ModifyConstantValueCoroutine(float value, bool isAddition)
    {
        while (true)
        {
            float deltaValue = value * _delta;

            if (isAddition)
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

    private void OnValueExhausted(GameObject exhaustedObject)
    {
        ValueExhausted?.Invoke(gameObject);
    }

    private void OnMaxValueInitialized()
    {
        MaxValueInitialized?.Invoke(_maxValue);
    }

    private void OnCurrentValueUpdated()
    {
        CurrentValueUpdated?.Invoke(_currentValue);
    }

    private void OnValueRemoved()
    {
        ValueRemoved?.Invoke();
    }
}
