using UnityEngine;

public class ForwardProjectile : MonoBehaviour
{
    [Header("VFX RELATED")]
    [SerializeField] private float _particleDelay;

    [Header("PROJECTILE VARIABLES")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _fireForce = 10f;
    private Rigidbody _rigidBody;

    private bool _active = true;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.AddForce(_fireForce * transform.forward, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!_active) return;

        if (collision.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(gameObject, _damage);
        }

        _active = false;
        _rigidBody.velocity = Vector3.zero;

        Invoke(nameof(DestroyInstance), _particleDelay);
    }

    private void DestroyInstance()
    {
        Destroy(gameObject);
    }
}
