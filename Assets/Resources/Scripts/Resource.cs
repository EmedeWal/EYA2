using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    [Header("RESOURCE VALUES")]
    [SerializeField] protected float MaxValue;
    [SerializeField] protected float StartingValue;
    protected float CurrentValue;

    protected void InitialiseValues()
    {
        CurrentValue = StartingValue;
    }

    protected void AddValue(float amount)
    {
        CurrentValue += amount;

        if (CurrentValue > MaxValue)
        {
            CurrentValue = MaxValue;
        }
    }

    protected void RemoveValue(float amount)
    {
        CurrentValue -= amount;

        if (CurrentValue < 0)
        {
            CurrentValue = 0;
        }
    }

    public bool AtMinValue()
    {
        return CurrentValue <= 0;
    }    

    public bool AtMaxValue()
    {
        return CurrentValue >= MaxValue;
    }
}
