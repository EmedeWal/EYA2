using UnityEngine;
using TMPro;

public class SoulsUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Souls _souls;

    public void Init()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _souls = Souls.Instance;
        _souls.CurrentValueUpdated += SoulsUI_CurrentValueUpdated;
    }

    private void SoulsUI_CurrentValueUpdated(int currentSouls)
    {
        _text.text = currentSouls.ToString();
    }
}
