using UnityEngine;

public class Health : Resource
{
    // Variables
    private float _damageModifier = 1;
    private bool _invincible = false;

    // Events
    public delegate void DeathDelegate(GameObject gameObject);
    public event DeathDelegate Death;

    public void Heal(float amount)
    {
        AddValue(amount);
    }

    public void TakeDamage(float amount)
    {
        if (_invincible) return;

        float finalDamage = amount * _damageModifier;
        RemoveValue(finalDamage);

        if (AtMinValue())
        {
            _invincible = true;
            OnDeath();
        }
        //Debug.Log($"{gameObject.name} has taken {finalDamage} damage. Currenthealth: {CurrentValue}");
    }

    public void HealOverTime(float totalAmount, float totalTime, float coroutineSpeed = 10)
    {
        StartCoroutine(AddValueOverTimeCoroutine(totalAmount, totalTime, coroutineSpeed));
    }

    public void SetDamageReduction(float damageReduction)
    {
        _damageModifier = 1 - damageReduction;
    }

    public void SetInvincible(bool active)
    {
        _invincible = active;
    }

    private void OnDeath()
    {
        Death?.Invoke(gameObject);
    }
}
