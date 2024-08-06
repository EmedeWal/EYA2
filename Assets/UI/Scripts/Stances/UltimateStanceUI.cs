public class UltimateStanceUI : StanceUI
{
    private UltimateUI _ultimateUI;
    private StanceType _currentStance;
    private bool _ultimateActive = false;

    private void Awake()
    {
        _ultimateUI = GetComponent<UltimateUI>();
    }

    protected override void StanceUI_UpdateStance(StanceType stanceType)
    {
        _currentStance = stanceType;
        if (!_ultimateActive) UpdateUI(_currentStance);
    }

    private void UltimateStanceUI_UltimateStart(float duration)
    {
        ActivateUltimate();
        Invoke(nameof(DeactivateUltimate), duration);
    }

    private void ActivateUltimate()
    {
        _ultimateActive = true;
    }

    private void DeactivateUltimate()
    {
        _ultimateActive = false;
        UpdateUI(_currentStance);
    }
}
