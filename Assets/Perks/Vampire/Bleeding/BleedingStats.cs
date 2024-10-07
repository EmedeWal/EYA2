using UnityEngine;

public class BleedingStats
{
    public float Damage { get; private set; }
    public float Duration { get; private set; }
    public int MaxStacks { get; private set; }

    public BleedingStats(float damage, float duration, int maxStacks)
    {
        Damage = damage;
        Duration = duration;
        MaxStacks = maxStacks;
    }
}