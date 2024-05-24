public class ManaPotionUI : PotionUI
{
    private void OnEnable()
    {
        ManaPotion.ManaPotionChargesChanged += ManaPotionUI_ManaPotionChargesChanged;
        ManaPotion.ManaPotionRefillStarted += ManaPotionUI_ManaPotionRefillStarted;
        ManaPotion.ManaPotionRefillUpdated += ManaPotionUI_ManaPotionRefillBoosted;
    }

    private void OnDisable()
    {
        ManaPotion.ManaPotionChargesChanged -= ManaPotionUI_ManaPotionChargesChanged;
        ManaPotion.ManaPotionRefillStarted += ManaPotionUI_ManaPotionRefillStarted;
        ManaPotion.ManaPotionRefillUpdated -= ManaPotionUI_ManaPotionRefillBoosted;
    }

    private void ManaPotionUI_ManaPotionChargesChanged(int currentCharges)
    {
        UpdateCharges(currentCharges);
    }

    private void ManaPotionUI_ManaPotionRefillStarted(float startTime)
    {
        StartRefillTime(startTime);
    }

    private void ManaPotionUI_ManaPotionRefillBoosted(float remainingTime)
    {
        UpdateRefillTime(remainingTime);
    }
}
