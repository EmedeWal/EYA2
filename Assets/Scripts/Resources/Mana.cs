public class Mana : Resource
{
    public void Gain(float amount)
    {
        AddValue(amount);
    }

    public void SpendMana(float amount)
    {
        RemoveValue(amount);
    }
}
