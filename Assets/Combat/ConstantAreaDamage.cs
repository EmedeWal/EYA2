using UnityEngine;

public class ConstantAreaDamage : MonoBehaviour
{
    private GameObject _gameObject;
    private Transform _transform;

    private Collider _colliderToIgnore;
    private LayerMask _targetLayer;
    private float _damage;
    private float _radius;

    public void Init(float radius, float damage, LayerMask targetLayer, Collider colliderToIgnore = null)
    {
        _gameObject = gameObject;
        _transform = transform;

        _colliderToIgnore = colliderToIgnore;
        _targetLayer = targetLayer;
        _damage = damage;
        _radius = radius;
    }

    public void Tick(float delta)
    {
        Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _targetLayer);

        foreach (Collider collider in colliders)
        {
            if (collider == _colliderToIgnore) continue;
            
            if (collider.TryGetComponent(out Health health))
            {
                health.TakeDamage(_gameObject, _damage * delta);
            }
        }
    }
}
