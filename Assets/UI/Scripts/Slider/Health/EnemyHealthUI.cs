public class EnemyHealthUI : HealthUI
{
    public void UpdateMaxHealth(float maxHealth)
    {
        SetMaxHealth(maxHealth);
    }

    public void UpdateCurrentHealth(float currentHealth)
    {
        SetCurrentHealth(currentHealth);
    }
}
