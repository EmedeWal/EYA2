using UnityEngine;
using UnityEngine.UI;

public abstract class FillUI : MonoBehaviour
{
    [Header("RADIAL TIMER SETTINGS")]
    [SerializeField] protected Image TimerImage;
    [SerializeField] private bool _reversed;
    private float _fillAmount;

    private float _maxValue;
    private float _currentValue;

    private void Start()
    {
        InitialiseFillAmount();
    }

    protected void SetMaxValue(float maxValue)
    {
        InitialiseFillAmount();
        _maxValue = maxValue;
    }

    protected void SetCurrentValue(float currentValue)
    {
        _currentValue = currentValue;

        if (_reversed) ReversedFill();
        else RegularFill();
    }

    private void RegularFill()
    {
        TimerImage.fillAmount = _currentValue / _maxValue;
    }

    private void ReversedFill()
    {
        TimerImage.fillAmount = 1 - _currentValue / _maxValue;
    }

    private void InitialiseFillAmount()
    {
        if (_reversed) _fillAmount = 1;
        else _fillAmount = 0;

        TimerImage.fillAmount = _fillAmount;
    }
}
