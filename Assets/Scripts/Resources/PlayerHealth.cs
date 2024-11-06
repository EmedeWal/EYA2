namespace EmeWillem
{
    using UnityEngine;

    public class PlayerHealth : Health
    {
        [Header("EVASION EFFECT")]
        [SerializeField] private Transform _center;
        [SerializeField] private VFX _evasionVFX;

        public float EvasionChance { private get; set; } = 0;
        public bool Resurrection { private get; set; } = false;
        public bool Shielded { private get; set; } = false;

        public delegate void HitShieldedDelegate(GameObject attackerObject, float damageAbsorbed);
        public event HitShieldedDelegate HitShielded;

        public delegate void ResurrectionEnabledDelegate(bool enabled);
        public event ResurrectionEnabledDelegate ResurrectionEnabled;

        public delegate void ResurrectedDelegate();
        public event ResurrectedDelegate Resurrected;

        public float TakeDamage(GameObject attackerObject, float amount)
        {
            //if (Utilities.GetChanceRoll(EvasionChance))
            //{
            //    HandleEvasion(attackerObject); return 0f;
            //}
            //else
            //{
            //    float finalDamage = amount * ((100 - DamageReduction) / 100);
            //    float damageDealt = Mathf.Min(finalDamage, CurrentHealth);

            //    if (Shielded) 
            //    { 
            //        OnHitShielded(attackerObject, damageDealt); return 0f; 
            //    }

            //    if (CurrentHealth <= finalDamage && Resurrection)
            //    {
            //        OnResurrected(); return 0f;
            //    }
            //    else
            //    {
            //        RemoveS(finalDamage);
            //    }

            //    return damageDealt;
            //}

            return 0;
        }

        public void EnableResurrection(bool enable)
        {
            OnResurrectionEnabled(enable);
            Resurrection = enable;
        }

        private void HandleEvasion(GameObject attackerObject)
        {
            Vector3 directionToAttacker = attackerObject.transform.position - transform.position;
            directionToAttacker.y = 0;

            VFX evasionVFX = VFXManager.Instance.AddStaticVFX(_evasionVFX, _center.position, Quaternion.LookRotation(directionToAttacker), 1f);

            AudioSource source = evasionVFX.GetComponent<AudioSource>();
            AudioSystem.Instance.PlayAudio(source, source.clip, source.volume);
        }

        private void OnHitShielded(GameObject attackerObject, float damageAbsorbed)
        {
            HitShielded?.Invoke(attackerObject, damageAbsorbed);
        }

        private void OnResurrectionEnabled(bool enabled)
        {
            ResurrectionEnabled?.Invoke(enabled);
        }

        private void OnResurrected()
        {
            Resurrected?.Invoke();
        }
    }
}