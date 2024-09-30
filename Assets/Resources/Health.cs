using UnityEngine;

public class Health : Resource
{
    public float DamageReduction { private get; set; } = 0;
    public float EvasionChance { private get; set; } = 0;
    public bool Invincible {  private get; set; } = false;

    public delegate void DeathDelegate(GameObject gameObject);
    public event DeathDelegate Death;

    public void Heal(float amount)
    {
        AddValue(amount);
    }

    public float TakeDamage(float amount)
    {
        if (Invincible || Helpers.GetChanceRoll(EvasionChance)) return 0f;  

        float finalDamage = amount * ((100 - DamageReduction) / 100);  
        float damageDealt = Mathf.Min(finalDamage, CurrentValue);

        RemoveValue(damageDealt);

        if (AtMinValue())
        {
            OnDeath();
        }

        return damageDealt;
    }

    private void OnDeath()
    {
        Death?.Invoke(gameObject);
    }
}
