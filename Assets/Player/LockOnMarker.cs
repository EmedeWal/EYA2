using UnityEngine.UI;
using UnityEngine;

public class LockOnMarker :SingletonBase
{
    #region Singleton
    public static LockOnMarker Instance;

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

    public void SetLockOnTarget(LockOnTarget lockOnTarget)
    {
        if (_health != null)
        {
            _health.CurrentValueUpdated -= LockOnMarker_CurrentValueUpdated;
        }

        _health = lockOnTarget._Health;
        _maxValue = _health._MaxValue;
        SetBackgroundFillAmount(_health._CurrentValue);
        _health.CurrentValueUpdated += LockOnMarker_CurrentValueUpdated;
    }

    private void LockOnMarker_CurrentValueUpdated(float currentValue)
    {
        SetBackgroundFillAmount(currentValue);
    }

    private void SetBackgroundFillAmount(float amount)
    {
        _background.fillAmount = amount / _maxValue;
    }
}
