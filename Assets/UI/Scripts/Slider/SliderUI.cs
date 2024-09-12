using UnityEngine.UI;
using UnityEngine;

public abstract class SliderUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    protected void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = _slider.maxValue;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    protected void SetValue(float value)
    {
        _slider.value = value;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
