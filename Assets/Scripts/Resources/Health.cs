using System.Collections;
using UnityEngine;
using System;

namespace EmeWillem
{
    public class Health : MonoBehaviour
    {
        public event Action<GameObject> HealthExhausted;
        public event Action<int> HealthUpdated;
        public event Action<int> HealthRemoved;

        public int MaximumHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        private int _pendingHealthChange;

        public virtual void Init(int maximumHealth)
        {
            MaximumHealth = maximumHealth;
            CurrentHealth = MaximumHealth;

            _pendingHealthChange = 0;

            OnHealthUpdated(CurrentHealth);
        }

        public virtual void LateTick()
        {
            if (_pendingHealthChange != 0)
            {
                CurrentHealth += _pendingHealthChange;

                if (_pendingHealthChange < 0)
                {
                    OnHealthRemoved(Mathf.Min(_pendingHealthChange, CurrentHealth));
                }

                if (CurrentHealth > MaximumHealth)
                {
                    CurrentHealth = MaximumHealth;
                }
                else if (CurrentHealth <= 0)
                {
                    CurrentHealth = 0;
                    OnHealthExhausted(gameObject);
                }

                _pendingHealthChange = 0;
                OnHealthUpdated(CurrentHealth);
            }
        }

        public virtual int RemoveHealth(int amount)
        {
            int finalAmount = amount; // Calculate damage resistances and such, but that can wait.
            int removedAmount = Mathf.Min(finalAmount, CurrentHealth);

            _pendingHealthChange -= finalAmount;
            return removedAmount;
        }

        //public virtual int RestoreHealth(int amount)
        //{
        //    int finalAmount = amount; // Healing modifiers not in effect currently
        //    int restoredAmount = Mathf.Min(finalAmount, MaximumHealth - CurrentHealth);

        //    _pendingHealthChange += restoredAmount;
        //    return restoredAmount;
        //}

        private void OnHealthExhausted(GameObject healthObject)
        {
            HealthExhausted?.Invoke(healthObject);
        }

        private void OnHealthUpdated(int currentHealth)
        {
            HealthUpdated?.Invoke(currentHealth);
        }

        private void OnHealthRemoved(int damageTaken)
        {
            HealthRemoved?.Invoke(damageTaken);
        }

        private IEnumerator HealthCoroutine(int amount)
        {
            yield return new WaitForSeconds(1);

            _pendingHealthChange += amount;
        }
    }
}