using UnityEngine;

public class ConstantAreaDamage : MonoBehaviour
{
    protected GameObject _GameObject;
    protected Transform _Transform;
    protected LayerMask _TargetLayer;
    protected float _Damage;
    protected float _Radius;

    public virtual void Init(float radius, float damage, LayerMask targetLayer)
    {
        _GameObject = gameObject;
        _Transform = transform;

        _TargetLayer = targetLayer;
        _Damage = damage;
        _Radius = radius;

        if (TryGetComponent(out AudioSource audioSource))
        {
            AudioSystem.Instance.PlayAudio(audioSource, audioSource.clip, audioSource.volume);
        }
    }

    public void Tick(float delta)
    {
        Collider[] colliders = Physics.OverlapSphere(_Transform.position, _Radius, _TargetLayer);

        foreach (Collider collider in colliders)
        {
            Effect(collider, delta);
        }
    }

    protected virtual void Effect(Collider collider, float delta)
    {
        if (collider.TryGetComponent(out Health health))
        {
            health.TakeDamage(_GameObject, _Damage * delta);
        }
    }
}