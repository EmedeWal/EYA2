using UnityEngine;

public class Health : Resource
{
    public float DamageReduction { private get; set; } = 0;
    public float EvasionChance { private get; set; } = 0;
    public bool Invincible {  private get; set; } = false;
    public bool Shielded { private get; set; } = false;

    public delegate void HitShieldedDelegate(GameObject attackerObject, float damageAbsorbed);
    public event HitShieldedDelegate HitShielded; 

    public void Heal(float amount)
    {
        AddValue(amount);
    }

    public float TakeDamage(GameObject attackerObject, float amount)
    {
        if (Invincible) return 0f;
        
        if (Helpers.GetChanceRoll(EvasionChance))
        {
            HandleEvasion(attackerObject); return 0f;
        }
        else
        {
            float finalDamage = amount * ((100 - DamageReduction) / 100);
            float damageDealt = Mathf.Min(finalDamage, CurrentValue);

            if (Shielded) { OnHitShielded(attackerObject, damageDealt); return 0f; }

            RemoveValue(damageDealt);

            return damageDealt;
        }
    }

    private void HandleEvasion(GameObject attackerObject)
    {

    }

    private void OnHitShielded(GameObject attackerObject, float damageAbsorbed)
    {
        HitShielded?.Invoke(attackerObject, damageAbsorbed);
    }
}
