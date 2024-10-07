using UnityEngine;

public class Explosion : AreaOfEffect
{
    [Header("DAMAGE")]
    [SerializeField] private float Damage;

    public void InitExplosion(float radius, float damage, LayerMask targetLayers, float audioOffset = 0, Collider colliderToIgnore = null)
    {
        Damage = damage;

        base.Init(radius, targetLayers, colliderToIgnore);

        if (TryGetComponent(out AudioSource audioSource))
        {
            AudioSystem.Instance.PlayAudioClip(audioSource, audioSource.clip, audioSource.volume, audioOffset);
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
