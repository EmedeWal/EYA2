using System.Collections.Generic;
using UnityEngine;

public class Bloodwave : MonoBehaviour
{
    [Header("STAT REFERENCE")]
    [SerializeField] private BloodwaveStats _bloodwaveStats;
    [SerializeField] private PlayerStats _playerStats;

    private List<GameObject> _hits = new();

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out Health health))
        {
            if (!_hits.Contains(other))
            {
                _hits.Add(other);

                float finalDamage = _bloodwaveStats.Damage;

                if (_bloodwaveStats.MultiplierEnabled)
                {
                    finalDamage *= _playerStats.GetCurrentStat(Stat.CriticalMultiplier);
                }

                health.TakeDamage(gameObject, finalDamage);
            }
        }
    }
}