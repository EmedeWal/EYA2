using UnityEngine.UI;
using UnityEngine;

public class LockMarker :SingletonBase
{
    #region Singleton
    public static LockMarker Instance;

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("UI REFERENCES")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;

    private Health _health;
    private float _maxValue;

    public void SetLockOnTarget(LockTarget target)
    {
        if (_health != null)
        {
            _health.CurrentValueUpdated -= LockMarker_CurrentValueUpdated;
        }

        _health = target.Health;
        _maxValue = _health.MaxValue;
        SetBackgroundFillAmount(_health.CurrentValue);
        _health.CurrentValueUpdated += LockMarker_CurrentValueUpdated;
    }

    private void LockMarker_CurrentValueUpdated(float currentValue)
    {
        SetBackgroundFillAmount(currentValue);
    }

    private void SetBackgroundFillAmount(float amount)
    {
        _background.fillAmount = amount / _maxValue;
    }
}
