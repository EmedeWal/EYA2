using UnityEngine;

public class HealthUI : SliderUI
{
    [Header("REFERENCES")]
    [SerializeField] private Health _health;

    private void OnEnable()
    {
        _health.MaxValueInitialized += HealthUI_MaxValueInitialized;
        _health.CurrentValueUpdated += HealthUI_CurrentValueUpdated;
    }

    private void OnDisable()
    {
        _health.MaxValueInitialized -= HealthUI_MaxValueInitialized;
        _health.CurrentValueUpdated -= HealthUI_CurrentValueUpdated;
    }

    private void HealthUI_MaxValueInitialized(float maxHealth)
    {
        SetMaxValue(maxHealth);
    }

    private void HealthUI_CurrentValueUpdated(float currentHealth)
    {
        SetValue(currentHealth);
    }
}
