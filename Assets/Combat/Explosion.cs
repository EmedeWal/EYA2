using UnityEngine;

public class Explosion : AreaOfEffect
{
    [Header("VARIABLES")]
    [SerializeField] private float _damage;

    public override void Init(float radius, LayerMask targetLayers, Collider colliderToIgnore = null)
    {
        base.Init(radius, targetLayers, colliderToIgnore);

        if (TryGetComponent(out AudioSource audioSource))
        {
            AudioSystem.Instance.PlayAudioClip(audioSource, audioSource.clip, audioSource.volume);
        }
    }

    protected override void Effect(Collider hit)
    {
        if (hit.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(gameObject, _damage);
        }
    }
}
