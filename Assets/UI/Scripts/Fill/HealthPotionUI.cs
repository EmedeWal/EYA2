public class HealthPotionUI : PotionUI
{
    private void OnEnable()
    {
        HealthPotion.HealthPotionChargesChanged += HealthPotionUI_HealthPotionChargesChanged;
        HealthPotion.HealthPotionRefillStarted += HealthPotionUI_HealthPotionRefillStarted;
        HealthPotion.HealthPotionRefillBoosted += HealthPotionUI_HealthPotionRefillBoosted;
    }

    private void OnDisable()
    {
        HealthPotion.HealthPotionChargesChanged -= HealthPotionUI_HealthPotionChargesChanged;
        HealthPotion.HealthPotionRefillStarted -= HealthPotionUI_HealthPotionRefillStarted;
        HealthPotion.HealthPotionRefillBoosted -= HealthPotionUI_HealthPotionRefillBoosted;
    }

    private void HealthPotionUI_HealthPotionChargesChanged(int charges)
    {
        UpdateCharges(charges);
    }

    private void HealthPotionUI_HealthPotionRefillStarted(float refillTime)
    {
        StartRefillTime(refillTime);
    }

    private void HealthPotionUI_HealthPotionRefillBoosted(float boost)
    {
        UpdateRefillTime(boost);
    }
}
