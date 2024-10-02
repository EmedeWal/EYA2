using UnityEngine;

public abstract class AreaOfEffect : MonoBehaviour
{
    [Header("GIZMOS. INSPECTOR ONLY")]
    public float _radius = 4;

    public void Init(float radius, LayerMask targetLayers, Collider colliderToIgnore = null)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayers);

        foreach (Collider hit in hits)
        {
            if (hit != colliderToIgnore) Effect(hit);
        }
    }

    protected abstract void Effect(Collider hit);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
