using UnityEngine;

public class Health : Resource
{
    [Header("EVASION EFFECT")]
    [SerializeField] private Transform _center;
    [SerializeField] private VFX _evasionVFX;

    private bool _resurrection = false;

    public float DamageReduction { private get; set; } = 0;
    public float EvasionChance { private get; set; } = 0;
    public bool Shielded { private get; set; } = false;

    public delegate void HitShieldedDelegate(GameObject attackerObject, float damageAbsorbed);
    public event HitShieldedDelegate HitShielded;

    public delegate void ResurrectionEnabledDelegate(bool enabled);
    public event ResurrectionEnabledDelegate ResurrectionEnabled;

    public delegate void ResurrectedDelegate();
    public event ResurrectedDelegate Resurrected;

    public void Heal(float amount)
    {
        AddValue(amount);
    }

    public float TakeDamage(GameObject attackerObject, float amount)
    {
        if (Helpers.GetChanceRoll(EvasionChance))
        {
            HandleEvasion(attackerObject); return 0f;
        }
        else
        {
            float finalDamage = amount * ((100 - DamageReduction) / 100);
            float damageDealt = Mathf.Min(finalDamage, CurrentValue);

            if (Shielded) { OnHitShielded(attackerObject, damageDealt); return 0f; }

            if (CurrentValue <= damageDealt && _resurrection)
            {
                OnResurrected(); return 0f;
            }
            else
            {
                RemoveValue(damageDealt);
            }

            return damageDealt;
        }
    }

    public void EnableResurrection(bool enable)
    {
        OnResurrectionEnabled(enable);
        _resurrection = enable;
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
