using UnityEngine;

public class Explosion : AreaOfEffect
{
    [Header("VARIABLES")]
    [SerializeField] private float _damage;

    protected override void Effect(Collider hit)
    {
        if (hit.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(gameObject, _damage);
        }
    }
}
