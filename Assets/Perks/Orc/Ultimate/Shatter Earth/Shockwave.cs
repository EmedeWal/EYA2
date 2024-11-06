namespace EmeWillem
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Shockwave : MonoBehaviour
    {
        private Dictionary<float, float> _shockWaveData = new Dictionary<float, float>();
        private LayerMask _targetLayer;
        private float _scaling = 1;

        public void Init(LayerMask targetLayer, float scaling)
        {
            _shockWaveData.Add(9f, 10f);
            _shockWaveData.Add(6f, 20f);
            _shockWaveData.Add(3f, 30f);

            _targetLayer = targetLayer;
            _scaling = scaling;

            CastShockwave();
        }

        private void CastShockwave()
        {
            foreach (KeyValuePair<float, float> shockWave in _shockWaveData)
            {
                float radius = shockWave.Key;
                float baseDamage = shockWave.Value;

                CastShockwave(radius, baseDamage);
            }
        }

        private void CastShockwave(float radius, float baseDamage)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, _targetLayer);

            foreach (Collider hit in hits)
            {
                float distanceToHit = Vector3.Distance(transform.position, hit.transform.position);
                float damage = baseDamage;

                if (_scaling > 1)
                {
                    float proximityFactor = 1f - (distanceToHit / radius);
                    float damageMultiplier = 1f + (_scaling - 1) * proximityFactor;
                    damage *= damageMultiplier;
                }

                if (hit.TryGetComponent<Health>(out var health))
                {
                    //health.TakeDamage(gameObject, damage);
                }
            }
        }
    }

}