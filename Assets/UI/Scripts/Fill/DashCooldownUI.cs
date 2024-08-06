using UnityEngine;

public class DashCooldownUI : FillUI
{
    [Header("REFERENCE")]
    [SerializeField] private PlayerDash _playerDash;

    private void OnEnable()
    {
        _playerDash.DashCooldownStart += DashCooldownUI_DashCooldownStart;
        _playerDash.DashCooldownUpdate += DashCooldownUI_DashCooldownUpdate;
    }

    private void OnDisable()
    {
        _playerDash.DashCooldownStart -= DashCooldownUI_DashCooldownStart;
        _playerDash.DashCooldownUpdate -= DashCooldownUI_DashCooldownUpdate;
    }

    private void DashCooldownUI_DashCooldownStart(float maxValue)
    {
        SetMaxValue(maxValue);
    }

    private void DashCooldownUI_DashCooldownUpdate(float currentValue)
    {
        SetCurrentValue(currentValue);
    }
}
