public class ManaPotion : Potion, IPotion
{
    private Mana _mana;

    private void Awake()
    {
        _mana = GetComponentInParent<Mana>();
    }

    private void Start()
    {
        SetCharges(_MaxCharges);
    }

    public void Consume()
    {
        if (CanConsume())
        {
            ConsumePotion();
        }
    }

    protected override void TriggerPotionEffect()
    {
        base.TriggerPotionEffect();
        _mana.GainManaOverTime(_RefillAmount, _RefillTime, _RefillSpeed);
    }

    private bool CanConsume()
    {
        return _CurrentCharges > 0 && !_mana.AtMaxValue();
    }
}
