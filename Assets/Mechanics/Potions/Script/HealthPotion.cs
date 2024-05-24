using UnityEngine;

public class HealthPotion : Potion, IPotion
{
    [Header("REFERENCE")]
    [SerializeField] private PlayerHealth _playerHealth;

    public delegate void HealthPotion_ChargesChanged(int charges);
    public static event HealthPotion_ChargesChanged HealthPotionChargesChanged;

    public delegate void HealthPotion_RefillStarted(float startTime);
    public static event HealthPotion_RefillStarted HealthPotionRefillStarted;

    public delegate void HealthPotion_RefillUpdated(float remainingTime);
    public static event HealthPotion_RefillUpdated HealthPotionRefillUpdated;

    private void Start()
    {
        SetCharges();
    }

    protected override void ChargesChanged(int currentCharges)
    {
        OnChargesChanged(currentCharges);
    }

    protected override void RefillStarted(float startTime)
    {
        OnRefillStarted(startTime);
    }

    protected override void RefillUpdated(float remainingTime)
    {
        OnRefillUpdated(remainingTime);
    }

    private void OnChargesChanged(int currentCharges)
    {
        HealthPotionChargesChanged?.Invoke(currentCharges);
    }

    private void OnRefillStarted(float startTime)
    {
        HealthPotionRefillStarted?.Invoke(startTime);
    }    

    private void OnRefillUpdated(float remainingTime)
    {
        HealthPotionRefillUpdated?.Invoke(remainingTime);
    }

    public void Consume()
    {
        if (CanConsume())
        {
            ConsumePotion();
            TriggerPotionEffect();
        }
    }

    private bool CanConsume()
    {
        return CurrentCharges > 0 && !_playerHealth.AtMaxValue();
    }

    private void TriggerPotionEffect()
    {
        _playerHealth.HealOverTime(RefillAmount);
    }
}