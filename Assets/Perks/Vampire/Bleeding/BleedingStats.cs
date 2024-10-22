public class BleedingStats
{
    public int MaxStacks { get; private set; }
    public float Damage { get; private set; }
    public float Duration { get; private set; }
    public float DamageReductionModifier { get; private set; }
    public float DamageInflictedModifier { get; private set; }

    public BleedingStats(int maxStacks, float damage, float duration, float damageReductionModifier, float damageInflictedModifier)
    {
        MaxStacks = maxStacks;

        Damage = damage;
        Duration = duration;
        DamageReductionModifier = damageReductionModifier;
        DamageInflictedModifier = damageInflictedModifier;
    }
}