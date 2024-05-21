using UnityEngine;
using UnityEngine.UI;

public abstract class SliderUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Gradient gradient;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    protected void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = slider.maxValue;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    protected void SetValue(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
