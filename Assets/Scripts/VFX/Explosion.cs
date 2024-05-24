using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Variables")]
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private float _damage = 25;
    [SerializeField] private float _radius = 5;

    private Collider _colliderToIgnore;

    private void Start()
    {
        CastExplosion();
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void CastExplosion()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _targetLayers);

        foreach (Collider hit in hits)
        {
            if (hit != _colliderToIgnore)
            {
                if (hit.TryGetComponent<Health>(out var health)) health.TakeDamage(_damage);
            }
        }
    }

    public void SetColliderToIgnore(Collider collider)
    {
        _colliderToIgnore = collider;
    }
}
