using UnityEngine;

public class ManaPotion : Potion, IPotion
{
    [Header("REFERENCE")]
    [SerializeField] private PlayerMana _playerMana;

    public delegate void ManaPotion_ChargesChanged(int currentCharges);
    public static event ManaPotion_ChargesChanged ManaPotionChargesChanged;

    public delegate void ManaPotion_RefillStarted(float startTime);
    public static event ManaPotion_RefillStarted ManaPotionRefillStarted;

    public delegate void ManaPotion_RefillUpdated(float remainingTime);
    public static event ManaPotion_RefillUpdated ManaPotionRefillUpdated;

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
        ManaPotionChargesChanged?.Invoke(currentCharges);
    }

    private void OnRefillStarted(float startTime)
    {
        ManaPotionRefillStarted?.Invoke(startTime);
    }

    private void OnRefillUpdated(float boost)
    {
        ManaPotionRefillUpdated?.Invoke(boost);
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
        return CurrentCharges > 0 && !_playerMana.AtMaxValue();
    }

    private void TriggerPotionEffect()
    {
        _playerMana.GainManaOverTime(RefillAmount);
    }
}
