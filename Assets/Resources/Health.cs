using UnityEngine;

public class Health : Resource
{
    [HideInInspector] public float DamageReduction;

    public void Heal(float amount)
    {
        AddValue(amount);
    }

    public virtual float TakeDamage(GameObject attackerObject, float amount)
    {
        float finalDamage = amount * ((100 - DamageReduction) / 100);
        float damageDealt = Mathf.Min(finalDamage, CurrentValue);

        RemoveValue(finalDamage);

        return damageDealt;
    }
}