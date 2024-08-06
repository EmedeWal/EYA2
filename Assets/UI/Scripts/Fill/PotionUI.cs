using UnityEngine;
using TMPro;

public class PotionUI : FillUI
{
    [Header("POTION REFERENCE")]
    [SerializeField] private Potion _potion;

    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        _potion.ChargesUpdated += PotionUI_ChargesUpdated;
        _potion.RefillStarted += PotionUI_RefillStarted;
        _potion.RefillUpdated += PotionUI_RefillUpdated;
    }

    private void OnDisable()
    {
        _potion.ChargesUpdated -= PotionUI_ChargesUpdated;
        _potion.RefillStarted -= PotionUI_RefillStarted;
        _potion.RefillUpdated -= PotionUI_RefillUpdated;
    }

    private void PotionUI_ChargesUpdated(int charges)
    {
        text.text = charges.ToString();
    }

    private void PotionUI_RefillStarted(float startTime)
    {
        SetMaxValue(startTime);
    }

    private void PotionUI_RefillUpdated(float remainingTime)
    {
        SetCurrentValue(remainingTime);
    }
}
