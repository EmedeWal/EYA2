public class DashCooldownUI : FillUI
{
    private void OnEnable()
    {
        PlayerDash.CooldownCountdown += DashCooldownUI_CooldownCountdown;
        PlayerDash.CooldownStart += DashCooldownUI_CooldownStart;
    }

    private void OnDisable()
    {
        PlayerDash.CooldownCountdown -= DashCooldownUI_CooldownCountdown;
        PlayerDash.CooldownStart -= DashCooldownUI_CooldownStart;
    }

    private void DashCooldownUI_CooldownStart(float maxValue)
    {
        SetMaxValue(maxValue);
    }

    private void DashCooldownUI_CooldownCountdown(float currentValue)
    {
        SetCurrentValue(currentValue);
    }
}
