using UnityEngine;

public class ManaUI : SliderUI
{
    [Header("REFERENCES")]
    [SerializeField] private Mana _mana;

    private void OnEnable()
    {
        _mana.MaxValueInitialized += ManaUI_MaxValueInitialized;
        _mana.CurrentValueUpdated += ManaUI_CurrentValueUpdated;
    }

    private void OnDisable()
    {
        _mana.MaxValueInitialized -= ManaUI_MaxValueInitialized;
        _mana.CurrentValueUpdated -= ManaUI_CurrentValueUpdated;
    }

    private void ManaUI_MaxValueInitialized(float maxMana)
    {
        SetMaxValue(maxMana);
    }

    private void ManaUI_CurrentValueUpdated(float currentMana)
    {
        SetValue(currentMana);
    }
}
