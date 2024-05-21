public class PlayerManaUI : SliderUI
{
    private void OnEnable()
    {
        PlayerMana.MaxManaSet += SetMaxMana;
        PlayerMana.CurrentManaChanged += SetCurrentMana;
    }

    private void OnDisable()
    {
        PlayerMana.MaxManaSet -= SetMaxMana;
        PlayerMana.CurrentManaChanged -= SetCurrentMana;
    }

    private void SetMaxMana(float maxMana)
    {
        SetMaxValue(maxMana);
    }

    private void SetCurrentMana(float currentMana)
    {
        SetValue(currentMana);
    }
}
