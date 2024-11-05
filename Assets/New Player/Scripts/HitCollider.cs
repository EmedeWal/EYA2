using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class HitCollider : MonoBehaviour
{
    [Header("HITBOX TYPE")]
    public HitBoxType HitBoxType;

    private CapsuleCollider _capsuleCollider;
    private List<Health> _healthList;
    private Rigidbody _rigidbody;
    private float _damage;

    public void Init(LayerMask targetLayer)
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.isTrigger = true;
        _capsuleCollider.enabled = false;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _rigidbody.useGravity = false;

        _capsuleCollider.includeLayers = targetLayer;
    }

    public void Activate(float damage)
    {
        _healthList = new List<Health>();
        _damage = damage;

        _capsuleCollider.enabled = true;
    }

    public void Deactivate()
    {
        _capsuleCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
        {
            if (!_healthList.Contains(health))
            {
                health.TakeDamage(gameObject, _damage);
                _healthList.Add(health);
            }
        }
    }
}
