using UnityEngine;
using System;

public class Souls : SingletonBase
{
    #region Singleton
    public static Souls Instance;

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("VARIABLES")]
    [SerializeField] private int _startingValue;
    private int _maxValue = 9999;
    private int _currentValue;

    public event Action<int> CurrentValueUpdated;

    public void Init()
    {
        _currentValue = _startingValue;
        OnCurrentValueUpdated();
    }

    public void AddValue(int amount)
    {
        _currentValue += amount;

        if (_currentValue > _maxValue)
        {
            _currentValue = _maxValue;
        }

        OnCurrentValueUpdated();
    }

    public void RemoveValue(int amount)
    {
        if (CanAfford(amount))
        {
            _currentValue -= amount;

            OnCurrentValueUpdated();
        }
    }

    public bool CanAfford(int cost)
    {
        if (_currentValue < cost)
        {
            return false;
        }
        return true;
    }

    private void OnCurrentValueUpdated()
    {
        CurrentValueUpdated?.Invoke(_currentValue);
    }
}
