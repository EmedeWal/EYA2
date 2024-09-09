using System.Collections;
using UnityEngine;
using System;

public abstract class Resource : MonoBehaviour
{
    [Header("RESOURCE VALUES")]
    [SerializeField] protected float _maxValue;
    [SerializeField] protected float _startingValue;
    protected float _currentValue;

    public event Action<float> MaxValueInitialized;
    public event Action<float> CurrentValueUpdated;
    public event Action<float> ValueRemoved;
    public event Action CoroutineCompleted;

    public void Initialize()
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
        float initialValue = _currentValue;
        _currentValue -= amount;

        if (_currentValue < 0)
        {
            _currentValue = 0;
        }

        float amountRemoved = initialValue - _currentValue;
        OnValueRemoved(amountRemoved);
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

    protected virtual IEnumerator AddValueOverTimeCoroutine(float totalAmount, float totalTime, float coroutineSpeed)
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

    private void OnMaxValueInitialized()
    {
        MaxValueInitialized?.Invoke(_maxValue);
    }

    private void OnCurrentValueUpdated()
    {
        CurrentValueUpdated?.Invoke(_currentValue);
    }

    private void OnValueRemoved(float amount)
    {
        ValueRemoved?.Invoke(amount);
    }

    private void OnCoroutineCompleted()
    {
        CoroutineCompleted?.Invoke();
    }
}
