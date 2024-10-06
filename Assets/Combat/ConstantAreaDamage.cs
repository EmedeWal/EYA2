using UnityEngine;

public class ConstantAreaDamage : MonoBehaviour
{
    private GameObject _gameObject;
    private Transform _transform;

    private Collider _colliderToIgnore;
    private LayerMask _targetLayer;
    private float _damage;
    private float _radius;

    public virtual void Init(float radius, float damage, LayerMask targetLayer, Collider colliderToIgnore = null)
    {
        _gameObject = gameObject;
        _transform = transform;

        _colliderToIgnore = colliderToIgnore;
        _targetLayer = targetLayer;
        _damage = damage;
        _radius = radius;

        if (TryGetComponent(out AudioSource audioSource))
        {
            AudioSystem.Instance.PlayAudioClip(audioSource, audioSource.clip, audioSource.volume);
        }
    }

    public void Tick(float delta)
    {
        Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _targetLayer);

        foreach (Collider collider in colliders)
        {
            if (collider == _colliderToIgnore) continue;

            Effect(collider, delta);
        }
    }

    protected virtual void Effect(Collider collider, float delta)
    {
        if (collider.TryGetComponent(out Health health))
        {
            health.TakeDamage(_gameObject, _damage * delta);
        }
    }
}
