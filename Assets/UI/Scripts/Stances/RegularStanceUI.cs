public class RegularStanceUI : StanceUI
{
    private void OnEnable()
    {
        Stance.UpdateStance += RegularStanceUI_UpdateStance;
    }

    private void OnDisable()
    {
        Stance.UpdateStance -= RegularStanceUI_UpdateStance;
    }

    private void RegularStanceUI_UpdateStance(StanceType stanceType)
    {
        UpdateUI(stanceType);
    }
}
