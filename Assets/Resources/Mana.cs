public class Mana : Resource
{
    private void Start()
    {
        InitializeValues();
    }

    public void GainMana(float amount)
    {
        AddValue(amount);
    }

    public void SpendMana(float amount)
    {
        RemoveValue(amount);
    }

    public void GainManaOverTime(float totalAmount, float totalTime, float coroutineSpeed = 10)
    {
        StartCoroutine(AddValueOverTimeCoroutine(totalAmount, totalTime, coroutineSpeed));
    }
}