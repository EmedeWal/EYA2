using UnityEngine;

public class Explosion : AreaOfEffect
{
    [Header("DAMAGE")]
    [SerializeField] private float Damage;

    public void InitExplosion(float radius, float damage, LayerMask targetLayers, Collider colliderToIgnore = null, float audioOffset = 0)
    {
        Damage = damage;

        base.Init(radius, targetLayers, colliderToIgnore);

        if (TryGetComponent(out AudioSource audioSource))
        {
            AudioSystem.Instance.PlayAudio(audioSource, audioSource.clip, audioSource.volume, audioOffset);
        }
    }

    protected override void Effect(Collider hit)
    {
        if (hit.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(gameObject, Damage);
        }
    }
}