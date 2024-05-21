public class PlayerHealthUI : HealthUI
{
    private void OnEnable()
    {
        PlayerHealth.MaxHealthSet += UpdateMaxHealth;
        PlayerHealth.CurrentHealthChanged += UpdateCurrentHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.MaxHealthSet -= UpdateMaxHealth;
        PlayerHealth.CurrentHealthChanged -= UpdateCurrentHealth;
    }

    private void UpdateMaxHealth(float maxHealth)
    {
        SetMaxHealth(maxHealth);
    }

    private void UpdateCurrentHealth(float currentHealth)
    {
        SetCurrentHealth(currentHealth);
    }
}
