using UnityEngine;
using TMPro;

public abstract class PotionUI : FillUI
{
    [SerializeField] private TextMeshProUGUI text;

    protected void UpdateCharges(int charges)
    {
        text.text = charges.ToString();
    }

    protected void StartRefillTime(float startTime)
    {
        SetMaxValue(startTime);
    }

    protected void UpdateRefillTime(float remainingTime)
    {
        SetCurrentValue(remainingTime);
    }
}
