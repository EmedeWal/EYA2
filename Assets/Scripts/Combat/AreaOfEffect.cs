using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AreaOfEffect : MonoBehaviour
{
    [Header("GIZMOS. INSPECTOR ONLY")]
    public float _radius = 4;

    public event Action<int> HitsDetected;

    public virtual void Init(float radius, LayerMask targetLayers, Collider colliderToIgnore = null)
    {
        List<Collider> hits = new();
        hits.AddRange(Physics.OverlapSphere(transform.position, radius, targetLayers));

        if (colliderToIgnore != null && hits.Contains(colliderToIgnore))
        {
            hits.Remove(colliderToIgnore);
        }

        OnHitsDetected(hits.Count);

        foreach (Collider hit in hits)
        {
            Effect(hit);
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