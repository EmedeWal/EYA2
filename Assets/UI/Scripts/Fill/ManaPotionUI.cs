public class ManaPotionUI : PotionUI
{
    private void OnEnable()
    {
        ManaPotion.ManaPotionChargesChanged += ManaPotionUI_ManaPotionChargesChanged;
        ManaPotion.ManaPotionRefillStarted += ManaPotionUI_ManaPotionRefillStarted;
        ManaPotion.ManaPotionRefillBoosted += ManaPotionUI_ManaPotionRefillBoosted;
    }

    private void OnDisable()
    {
        ManaPotion.ManaPotionChargesChanged -= ManaPotionUI_ManaPotionChargesChanged;
        ManaPotion.ManaPotionRefillStarted -= ManaPotionUI_ManaPotionRefillStarted;
        ManaPotion.ManaPotionRefillBoosted -= ManaPotionUI_ManaPotionRefillBoosted;
    }

    private void ManaPotionUI_ManaPotionChargesChanged(int charges)
    {
        UpdateCharges(charges);
    }

    private void ManaPotionUI_ManaPotionRefillStarted(float refillTime)
    {
        StartRefillTime(refillTime);
    }

    private void ManaPotionUI_ManaPotionRefillBoosted(float boost)
    {
        UpdateRefillTime(boost);
    }
}
