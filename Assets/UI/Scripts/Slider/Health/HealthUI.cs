public abstract class HealthUI : SliderUI
{
    protected void SetMaxHealth(float maxHealth)
    {
        SetMaxValue(maxHealth);
    }

    protected void SetCurrentHealth(float currentHealth)
    {
        SetValue(currentHealth);
    }
}
