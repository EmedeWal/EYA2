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

    public float TakeDamage(float amount)
    {
        if (_invincible) return 0f;  

        float finalDamage = amount * _damageModifier;  
        float damageDealt = Mathf.Min(finalDamage, _CurrentValue);

        RemoveValue(damageDealt);

        if (AtMinValue())
        {
            OnDeath();
        }

        return damageDealt;
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
