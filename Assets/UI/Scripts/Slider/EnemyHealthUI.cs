using UnityEngine;
using TMPro;

public class EnemyHealthUI : HealthUI
{
    [Header("BLEED SYSTEM")]
    [SerializeField] private TextMeshProUGUI _bleedText;
    [SerializeField] private GameObject _bleedIcon;
    private int _bleedStacks = 0;

    private void Start()
    {
        _bleedIcon.SetActive(false);
    }

    public void UpdateMaxHealth(float maxHealth)
    {
        //HealthUI_MaxValueInitialized(maxHealth);
    }

    public void UpdateCurrentHealth(float currentHealth)
    {
        //HealthUI_CurrentValueUpdated(currentHealth);
    }

    public void AddBleed()
    {
        ModifyBleedStacks(1);

        _bleedIcon.SetActive(true);
    }

    public void RemoveBleed()
    {
        ModifyBleedStacks(-1);

        if (_bleedStacks == 0 )
        {
            _bleedIcon.SetActive(false);
        }
    }

    private void ModifyBleedStacks(int modification)
    {
        _bleedStacks += modification;
        _bleedText.text = _bleedStacks.ToString();
    }
}
