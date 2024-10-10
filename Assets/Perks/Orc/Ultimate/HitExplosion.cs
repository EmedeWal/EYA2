using UnityEngine;

public class HitExplosion : AreaOfEffect
{
    [Header("DAMAGE")]
    [SerializeField] private float _damage;

    [Header("VISUALISATION")]
    [SerializeField] private VFX _hitVFX;
    private VFXManager _VFXManager;

    public void InitHitExplosion(float radius, float damage, LayerMask targetLayers, VFXManager VFXManager)
    {
        _damage = damage;
        _VFXManager = VFXManager;

        base.Init(radius, targetLayers);

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
            
            Transform transform = hit.transform;
            _VFXManager.AddVFX(_hitVFX, true, 1f, transform.position, transform.rotation);
        }
    }
}
