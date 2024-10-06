using UnityEngine.UI;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("HEALTH REFERENCE")]
    [SerializeField] private Health _health;

    [Header("IMAGE REFERENCE")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    private float _maxHealth;

    private void OnEnable()
    {
        _health.MaxValueInitialized += HealthUI_MaxValueInitialized;
        _health.CurrentValueUpdated += HealthUI_CurrentValueUpdated;
        _health.ResurrectionEnabled += HealthUI_ResurrectionEnabled;
    }

    private void OnDisable()
    {
        _health.MaxValueInitialized -= HealthUI_MaxValueInitialized;
        _health.CurrentValueUpdated -= HealthUI_CurrentValueUpdated;
        _health.ResurrectionEnabled -= HealthUI_ResurrectionEnabled;
    }

    private void HealthUI_MaxValueInitialized(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    private void HealthUI_CurrentValueUpdated(float currentHealth)
    {
        _background.fillAmount = currentHealth / _maxHealth;
    }

    private void HealthUI_ResurrectionEnabled(bool enabled)
    {
        if (enabled)
        {
            _icon.color = Color.green;
        }
        else
        {
            _icon.color = Color.white;
        }
    }
}
