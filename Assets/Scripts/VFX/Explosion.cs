using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Variables")]
    [SerializeField] private LayerMask _targetLayers;
    public float damage = 25;
    public float radius = 5;

    private Collider _colliderToIgnore;

    private void Start()
    {
        CastExplosion();
    }

    private void CastExplosion()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, _targetLayers);

        foreach (Collider hit in hits)
        {
            if (hit != _colliderToIgnore)
            {
                if (hit.TryGetComponent<Health>(out var health)) health.TakeDamage(damage);
            }
        }
    }

    public void SetColliderToIgnore(Collider collider)
    {
        _colliderToIgnore = collider;
    }
}
