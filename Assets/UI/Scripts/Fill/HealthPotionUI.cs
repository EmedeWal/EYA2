public class HealthPotionUI : PotionUI
{
    private void OnEnable()
    {
        HealthPotion.HealthPotionChargesChanged += HealthPotionUI_HealthPotionChargesChanged;
        HealthPotion.HealthPotionRefillStarted += HealthPotionUI_HealthPotionRefillStarted;
        HealthPotion.HealthPotionRefillUpdated += HealthPotionUI_HealthPotionRefillUpdated;
    }

    private void OnDisable()
    {
        HealthPotion.HealthPotionChargesChanged -= HealthPotionUI_HealthPotionChargesChanged;
        HealthPotion.HealthPotionRefillStarted -= HealthPotionUI_HealthPotionRefillStarted;
        HealthPotion.HealthPotionRefillUpdated -= HealthPotionUI_HealthPotionRefillUpdated;
    }

    private void HealthPotionUI_HealthPotionChargesChanged(int currentCharges)
    {
        UpdateCharges(currentCharges);
    }

    private void HealthPotionUI_HealthPotionRefillStarted(float startTime)
    {
        StartRefillTime(startTime);
    }

    private void HealthPotionUI_HealthPotionRefillUpdated(float remainingTime)
    {
        UpdateRefillTime(remainingTime);
    }
}
