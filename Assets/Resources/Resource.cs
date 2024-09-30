using System.Collections;
using UnityEngine;
using System;

public abstract class Resource : MonoBehaviour
{
    private float _maxValue;
    private float _currentValue;

    public event Action<float> MaxValueInitialized;
    public event Action<float> CurrentValueUpdated;
    public event Action CoroutineCompleted;
    public event Action ValueRemoved;

    public float MaxValue => _maxValue; 
    public float CurrentValue => _currentValue;
    public float RestorationModifier { private get; set; } = 1;

    public virtual void Init(float maxValue, float currentValue)
    {
        _maxValue = maxValue;
        _currentValue = currentValue;

        if (_maxValue < _currentValue)
        {
            Debug.LogWarning($"Current value exceeded max value in {gameObject.name}");
        }

        OnMaxValueInitialized();
        OnCurrentValueUpdated();
    }

    protected void AddValue(float amount)
    {
        _currentValue += amount;

        if (_currentValue > _maxValue)
        {
            _currentValue = _maxValue;
        }

        OnCurrentValueUpdated();
    }

    protected void RemoveValue(float amount)
    {
        _currentValue -= amount;

        if (_currentValue < 0)
        {
            _currentValue = 0;
        }

        OnCurrentValueUpdated();
        OnValueRemoved();
    }

    public bool AtMinValue()
    {
        return _currentValue <= 0;
    }    

    public bool AtMaxValue()
    {
        return _currentValue >= _maxValue;
    }

    public void AddValueOverTime(float totalAmount, float totalTime, float coroutineSpeed = 10)
    {
        StartCoroutine(AddValueOverTimeCoroutine());
        IEnumerator AddValueOverTimeCoroutine()
        {
            float valuePerSecond = totalAmount / totalTime;
            float valueIncrement = valuePerSecond / coroutineSpeed;
            float timeIncrement = 1 / coroutineSpeed;
            float valueAdded = 0;

            while (valueAdded < totalAmount)
            {
                AddValue(valueIncrement);
                valueAdded += valueIncrement;
                yield return new WaitForSeconds(timeIncrement);
            }

            OnCoroutineCompleted();
        }
    }

    public void AddConstantValue(float value, float ticks)
    {
        StartCoroutine(AddConstantValueCoroutine());
        IEnumerator AddConstantValueCoroutine()
        {
            float increment = value / ticks;
            float delay = 1 / ticks;

            while (true)
            {
                yield return new WaitForSeconds(delay);
                AddValue(increment * RestorationModifier);
            }
        }
    }

    private void OnMaxValueInitialized()
    {
        MaxValueInitialized?.Invoke(_maxValue);
    }

    private void OnCurrentValueUpdated()
    {
        CurrentValueUpdated?.Invoke(_currentValue);
    }

    private void OnCoroutineCompleted()
    {
        CoroutineCompleted?.Invoke();
    }

    private void OnValueRemoved()
    {
        ValueRemoved?.Invoke();
    }
}
