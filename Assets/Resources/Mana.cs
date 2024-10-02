public class Mana : Resource
{
    public void GainMana(float amount)
    {
        AddValue(amount);
    }

    public void SpendMana(float amount)
    {
        RemoveValue(amount);
    }
}
