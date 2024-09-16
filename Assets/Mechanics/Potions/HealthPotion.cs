public class HealthPotion : Potion, IPotion
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
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
        _health.AddValueOverTime(_RefillAmount, _RefillTime, _RefillSpeed);
    }

    private bool CanConsume()
    {
        return _CurrentCharges > 0 && !_health.AtMaxValue();
    }
}