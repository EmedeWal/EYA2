namespace EmeWillem
{
    using UnityEngine.UI;
    using UnityEngine;

    public class LockMarker : SingletonBase
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
                _health.HealthUpdated -= LockMarker_CurrentValueUpdated;
            }

            _health = target.Health;
            _maxValue = _health.MaximumHealth;
            SetBackgroundFillAmount(_health.CurrentHealth);
            _health.HealthUpdated += LockMarker_CurrentValueUpdated;
        }

        private void LockMarker_CurrentValueUpdated(int currentValue)
        {
            SetBackgroundFillAmount(currentValue);
        }

        private void SetBackgroundFillAmount(float amount)
        {
            _background.fillAmount = amount / _maxValue;
        }
    }

}