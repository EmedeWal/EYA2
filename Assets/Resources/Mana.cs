public class Mana : Resource
{
    private PlayerAttackHandler _playerAttackHandler;

    public override void Init(float maxValue, float currentValue)
    {
        base.Init(maxValue, currentValue);
    }

    public void GainMana(float amount)
    {
        AddValue(amount);
    }

    public void SpendMana(float amount)
    {
        RemoveValue(amount);
    }
}
