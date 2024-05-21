using UnityEngine;

public class HealthPotion : Potion, IPotion
{
    [Header("REFERENCE")]
    [SerializeField] private PlayerHealth _playerHealth;

    public delegate void Delegate_ChargesChanged(int charges);
    public static event Delegate_ChargesChanged HealthPotionChargesChanged;

    public delegate void Delegate_RefillStarted(float refillDuration);
    public static event Delegate_RefillStarted HealthPotionRefillStarted;

    public delegate void Delegate_RefillBoosted(float boost);
    public static event Delegate_RefillBoosted HealthPotionRefillBoosted;

    private void Start()
    {
        SetCharges();
    }

    private void OnEnable()
    {
        ChargesChanged += HealthPotion_ChargesChanged;
        RefillStarted += HealthPotion_RefillStarted;
        RefillBoosted += HealthPotion_RefillBoosted;
        PotionConsumed += HealthPotion_PotionConsumed;
    }

    private void OnDisable()
    {
        ChargesChanged -= HealthPotion_ChargesChanged;
        RefillStarted -= HealthPotion_RefillStarted;
        RefillBoosted -= HealthPotion_RefillBoosted;
        PotionConsumed -= HealthPotion_PotionConsumed;
    }

    private void HealthPotion_ChargesChanged(int currentCharges)
    {
        OnChargesChanged(currentCharges);
    }

    private void HealthPotion_RefillStarted(float cooldown)
    {
        OnRefillStarted(cooldown);
    }

    private void HealthPotion_RefillBoosted(float boost)
    {
        OnRefillBoosted(boost);
    }

    private void HealthPotion_PotionConsumed(float refillAmount)
    {
        TriggerPotionEffect(refillAmount);
    }

    private void OnChargesChanged(int currentCharges)
    {
        HealthPotionChargesChanged?.Invoke(currentCharges);
    }

    private void OnRefillStarted(float cooldown)
    {
        HealthPotionRefillStarted?.Invoke(cooldown);
    }

    private void OnRefillBoosted(float boost)
    {
        HealthPotionRefillBoosted?.Invoke(boost);
    }

    private void TriggerPotionEffect(float amount)
    { 
        _playerHealth.HealOverTime(amount);
    }

    public void Consume()
    {
        if (CanConsume()) ConsumePotion();
    }

    private bool CanConsume()
    {
        return CurrentCharges > 0 && !_playerHealth.AtMaxValue();
    }
}