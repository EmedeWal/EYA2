using UnityEngine;

public class ManaPotion : Potion, IPotion
{
    [Header("REFERENCE")]
    [SerializeField] private PlayerMana _playerMana;

    public delegate void Delegate_ChargesChanged(int charges);
    public static event Delegate_ChargesChanged ManaPotionChargesChanged;

    public delegate void Delegate_RefillStarted(float refillDuration);
    public static event Delegate_RefillStarted ManaPotionRefillStarted;

    public delegate void Delegate_RefillBoosted(float boost);
    public static event Delegate_RefillBoosted ManaPotionRefillBoosted;

    private void Start()
    {
        SetCharges();
    }

    private void OnEnable()
    {
        ChargesChanged += ManaPotion_ChargesChanged;
        RefillStarted += ManaPotion_RefillStarted;
        RefillBoosted += ManaPotion_RefillBoosted;
        PotionConsumed += ManaPotion_PotionConsumed;
    }

    private void OnDisable()
    {
        ChargesChanged -= ManaPotion_ChargesChanged;
        RefillStarted -= ManaPotion_RefillStarted;
        RefillBoosted += ManaPotion_RefillBoosted;
        PotionConsumed -= ManaPotion_PotionConsumed;
    }

    private void ManaPotion_ChargesChanged(int currentCharges)
    {
        OnChargesChanged(currentCharges);
    }

    private void ManaPotion_RefillStarted(float cooldown)
    {
        OnRefillStarted(cooldown);
    }

    private void ManaPotion_RefillBoosted(float boost)
    {
        OnRefillBoosted(boost);
    }

    private void ManaPotion_PotionConsumed(float refillAmount)
    {
        TriggerPotionEffect(refillAmount);
    }

    private void OnChargesChanged(int currentCharges)
    {
        ManaPotionChargesChanged?.Invoke(currentCharges);
    }

    private void OnRefillStarted(float cooldown)
    {
        ManaPotionRefillStarted?.Invoke(cooldown);
    }

    private void OnRefillBoosted(float boost)
    {
        ManaPotionRefillBoosted?.Invoke(boost);
    }

    private void TriggerPotionEffect(float amount)
    {
        _playerMana.GainManaOverTime(amount);
    }

    public void Consume()
    {
        if (CanConsume()) ConsumePotion();
    }

    private bool CanConsume()
    {
        return CurrentCharges > 0 && !_playerMana.AtMaxValue();
    }
}
