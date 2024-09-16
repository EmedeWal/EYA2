using UnityEngine.UI;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("HEALTH REFERENCE")]
    [SerializeField] private Health _health;

    [Header("IMAGE REFERENCE")]
    [SerializeField] private Image _image;
    private float _maxHealth;

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
        _maxHealth = maxHealth;
    }

    private void HealthUI_CurrentValueUpdated(float currentHealth)
    {
        _image.fillAmount = currentHealth / _maxHealth;
    }
}
