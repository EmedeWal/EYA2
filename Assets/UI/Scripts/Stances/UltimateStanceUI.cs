public class UltimateStanceUI : StanceUI
{
    private UltimateUI _ultimateUI;
    private StanceType _currentStance;
    private bool _ultimateActive = false;

    private void Awake()
    {
        _ultimateUI = GetComponent<UltimateUI>();
    }

    private void OnEnable()
    {
        Stance.UpdateStance += UltimateStanceUI_UpdateStance;
        Stance.UltimateStart += UltimateStanceUI_UltimateStart;
    }

    private void OnDisable()
    {
        Stance.UpdateStance -= UltimateStanceUI_UpdateStance;
        Stance.UltimateStart -= UltimateStanceUI_UltimateStart;
    }

    private void UltimateStanceUI_UpdateStance(StanceType stanceType)
    {
        _currentStance = stanceType;
        if (!_ultimateActive) UpdateUI(_currentStance);
    }

    private void UltimateStanceUI_UltimateStart(float duration)
    {
        ActivateUltimate();

        _ultimateUI.Duration(duration);

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
