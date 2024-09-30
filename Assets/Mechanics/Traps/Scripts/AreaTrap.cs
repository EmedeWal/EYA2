using UnityEngine;

public class AreaTrap : MonoBehaviour
{
    private float _damage = 0.2f;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Health>(out var health)) health.TakeDamage(gameObject, _damage);
    }
}
