using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private ProjectileData _projectileData;
    private VFX _currentVFX;

    private Rigidbody _rigidbody;
    private Collider _collider;

    public void Init(ProjectileData projectileData, Transform target, VFX currentVFX)
    {
        _projectileData = projectileData;
        _currentVFX = currentVFX;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;

        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;

        Vector3 velocity = _projectileData.Force * 50 * _projectileData.FirePoint.forward;
        _rigidbody.AddForce(velocity, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _projectileData.TargetLayer) != 0)
        {
            if (other.TryGetComponent<Health>(out var targetHealth))
            {
                targetHealth.TakeDamage(gameObject, _projectileData.Damage);
            }

            _projectileData.VFXManager.RemoveVFX(_currentVFX, 3f);
        }
    }
}
