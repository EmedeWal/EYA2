using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private Stat _stat;
    [SerializeField] private Color _color;
    private TextMeshProUGUI _text;
    private Image _fill;

    public Stat Stat => _stat;
    
    public void Init()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _fill = GetComponentInChildren<Image>();
    }

    public void UpdateUI(float value)
    {
        string formattedText = $"{Helpers.FormatStatName(_stat)} = {value}";
        formattedText += Helpers.GetStatLineEnd(_stat);

        _text.text = formattedText;
        _fill.color = _color;
    }
}
