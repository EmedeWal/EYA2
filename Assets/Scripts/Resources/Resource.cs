using System.Collections;
using UnityEngine;
using System;

public class Resource : MonoBehaviour
{
    public event Action<GameObject> ValueExhausted;
    public event Action<int> ValueUpdated;
    public event Action<int> ValueRemoved;

    public int MaximumValue { get; private set; }
    public int CurrentValue { get; private set; }

    private int _pendingValueChange;

    virtual public void Init(int maxHealth, int currentHealth)
    {
        MaximumValue = maxHealth;
        CurrentValue = currentHealth;

        _pendingValueChange = 0;

        OnValueUpdated(CurrentValue);
    }

    virtual public void LateTick()
    {
        if (_pendingValueChange != 0)
        {
            CurrentValue += _pendingValueChange;

            if (_pendingValueChange < 0)
            {
                OnValueRemoved(Mathf.Min(_pendingValueChange, CurrentValue));
            }

            if (CurrentValue > MaximumValue)
            {
                CurrentValue = MaximumValue;
            }
            else if (CurrentValue <= 0)
            {
                CurrentValue = 0;
                OnValueExhausted(gameObject);
            }

            _pendingValueChange = 0;
            OnValueUpdated(CurrentValue);
        }
    }

    virtual public int RemoveValue(int amount)
    {
        int finalAmount = amount; // Calculate damage resistances and such, but that can wait.
        int removedAmount = Mathf.Min(finalAmount, CurrentValue);

        _pendingValueChange -= finalAmount;
        return removedAmount;
    }

    virtual public int RestoreValue(int amount)
    {
        int finalAmount = amount; // Healing modifiers not in effect currently
        int restoredAmount = Mathf.Min(finalAmount, MaximumValue - CurrentValue);

        _pendingValueChange += restoredAmount;
        return restoredAmount;
    }

    private void OnValueExhausted(GameObject healthObject)
    {
        ValueExhausted?.Invoke(healthObject);
    }

    private void OnValueUpdated(int currentHealth)
    {
        ValueUpdated?.Invoke(currentHealth);
    }

    private void OnValueRemoved(int damageTaken)
    {
        ValueRemoved?.Invoke(damageTaken);
    }

    private IEnumerator ValueCoroutine(int amount)
    {
        yield return new WaitForSeconds(1);

        _pendingValueChange += amount;
    }
}