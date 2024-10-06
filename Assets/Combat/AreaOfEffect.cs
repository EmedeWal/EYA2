using UnityEngine;
using System;

public abstract class AreaOfEffect : MonoBehaviour
{
    [Header("GIZMOS. INSPECTOR ONLY")]
    public float _radius = 4;

    public event Action<int> HitsDetected;

    public virtual void Init(float radius, LayerMask targetLayers, Collider colliderToIgnore = null)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayers);

        OnHitsDetected(hits.Length);

        foreach (Collider hit in hits)
        {
            if (hit != colliderToIgnore) Effect(hit);
        }
    }

    protected abstract void Effect(Collider hit);

    private void OnHitsDetected(int hits)
    {
        HitsDetected?.Invoke(hits);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
