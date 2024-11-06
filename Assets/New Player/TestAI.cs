using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    public class TestAI : MonoBehaviour
    {
        [SerializeField] private DefenseCollider _defenseCollider;
        [SerializeField] private LockTarget _lockTarget;
        [SerializeField] private List<OffenseCollider> offenseColliders = new();

        private Health _health;
        private Posture _posture;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _posture = GetComponent<Posture>();

            _health.Init(1000);
            _posture.Init(new List<string> { "stagger" }, 3000, 10);
            _defenseCollider.Init(gameObject, _health, _posture);

            _lockTarget.Init(_health);

            foreach (OffenseCollider offense in offenseColliders)
            {
                offense.Init(transform, LayerMask.GetMask("Controller"));
            }
        }

        private void LateUpdate()
        {
            _health.LateTick();
            _posture.LateTick();
        }
    }

}