using System.Collections;
using UnityEngine;
using System;

public abstract class Resource : MonoBehaviour
{
    [Header("RESOURCE VALUES")]
    [SerializeField] private float _maxValue;
    [SerializeField] private float _startingValue;
    private float _currentValue;

    public event Action<float> MaxValueInitialized;
    public event Action<float> CurrentValueUpdated;
    public event Action CoroutineCompleted;

    public float _MaxValue => _maxValue; public float _CurrentValue => _currentValue;

    public virtual void Init()
    {
        _currentValue = _startingValue;
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
                AddValue(increment);
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
}
