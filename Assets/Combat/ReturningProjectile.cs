using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class ReturningProjectile : MonoBehaviour
{
    private ProjectileData _projectileData;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private VFX _currentVFX;

    private float _maxDistance;
    private float _stopDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _projectileData.TargetLayer) != 0)
        {
            if (other.TryGetComponent<Health>(out var targetHealth))
            {
                targetHealth.TakeDamage(gameObject, _projectileData.Damage);
            }
        }
    }

    public void Init(ProjectileData projectileData, VFX currentVFX, float stopDuration, float maxDistance)
    {
        _projectileData = projectileData;
        _stopDuration = stopDuration;
        _maxDistance = maxDistance;
        _currentVFX = currentVFX;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;

        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;

        StartCoroutine(MoveToPosition(OnReachedTarget, _projectileData.FirePoint.position + _projectileData.FirePoint.forward * _maxDistance, _projectileData.Force));
    }

    public void Cleanup()
    {
        _projectileData.VFXManager.RemoveVFX(_currentVFX);
    }

    private void OnReachedTarget()
    {
        StartCoroutine(StopAndReturn());
    }

    private IEnumerator MoveToPosition(Action onComplete, Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            _rigidbody.velocity = direction * speed;
            yield return null;
        }

        _rigidbody.velocity = Vector3.zero;
        onComplete?.Invoke();
    }

    private IEnumerator StopAndReturn()
    {
        _collider.isTrigger = false;
        yield return new WaitForSeconds(_stopDuration);

        _collider.isTrigger = true;
        StartCoroutine(MoveToPosition(Cleanup, _projectileData.FirePoint.position, _projectileData.Force));
    }
}