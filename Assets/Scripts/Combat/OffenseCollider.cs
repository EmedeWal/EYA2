using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class OffenseCollider : MonoBehaviour
    {
        public string Name { get; private set; }

        private List<GameObject> _attackTargetsList;
        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rigidbody;
        private Transform _transform;
        private LayerMask _targetLayer;
        private int _damage;
        private int _stagger;

        public void Init(Transform attackerTransform, LayerMask targetLayer)
        {
            Name = gameObject.name;

            _capsuleCollider = GetComponent<CapsuleCollider>();
            _capsuleCollider.isTrigger = true;
            _capsuleCollider.enabled = false;

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.useGravity = false;

            _transform = attackerTransform;
            _targetLayer = targetLayer;
        }

        public void Activate(int damage, int stagger)
        {
            _attackTargetsList = new();
            _damage = damage;
            _stagger = stagger;

            _capsuleCollider.enabled = true;
        }

        public void Deactivate()
        {
            _capsuleCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _targetLayer) != 0)
            {
                if (other.TryGetComponent(out DefenseCollider defenseCollider))
                {
                    if (!_attackTargetsList.Contains(defenseCollider.Parent))
                    {
                        _attackTargetsList.Add(defenseCollider.Parent);
                        defenseCollider.ProcessAttack(_transform.position, _damage, _stagger);
                    }
                }
            }
        }
    }
}