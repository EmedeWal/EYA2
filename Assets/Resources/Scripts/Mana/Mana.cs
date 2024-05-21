public abstract class Mana : Resource
{
    private void Start()
    {
        InitialiseValues();
        ManaInitiliased(MaxValue);
        ManaChanged(CurrentValue);
    }

    public void GainMana(float amount)
    {
        AddValue(amount);
        ManaChanged(CurrentValue);
    }

    public void SpentMana(float amount)
    {   
        RemoveValue(amount);
        ManaChanged(CurrentValue);
    }

    protected abstract void ManaInitiliased(float maxMana);

    protected abstract void ManaChanged(float currentMana);
}
