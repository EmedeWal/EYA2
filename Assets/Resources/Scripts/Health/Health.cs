using UnityEngine;

public class Health : Resource
{
    protected float DamageModifier = 1;
    protected bool Invincible = false;

    [Header("DEATH RELATED")]
    [SerializeField] private float _deathTime = 3f;

    private void Start()
    {
        InitialiseValues();
        HealthInitialised(MaxValue);
        HealthChanged(CurrentValue);
    }

    public void Heal(float amount)
    {
        AddValue(amount);
        HealthChanged(CurrentValue);
    }

    public void TakeDamage(float amount)
    {
        if (Invincible) return;

        float initialValue = CurrentValue;
        float finalDamage = amount * DamageModifier;

        RemoveValue(finalDamage);
        DamageTaken(initialValue - CurrentValue);

        //Debug.Log($"{gameObject.name} has taken {finalDamage} damage. Currenthealth: {CurrentValue}");

        if (AtMinValue())
        {
            Invincible = true;

            Invoke(nameof(DestroyGameObject), _deathTime);
            Death();

            return;
        }

        HealthChanged(CurrentValue);
    }

    protected virtual void HealthInitialised(float maxHealth) { return; }

    protected virtual void HealthChanged(float currentHealth) { return; }

    protected virtual void DamageTaken(float amount) { return; }

    protected virtual void Death() { return; }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
