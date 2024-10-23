using UnityEngine;

public class ProjectileData
{
    public VFXManager VFXManager;
    public Transform FirePoint;
    public VFX VFX;
    public LayerMask TargetLayer;
    public float Force;
    public float Damage;

    public ProjectileData(Transform firePoint, VFX vfx, LayerMask targetLayer, float force, float damage)
    {
        VFXManager = VFXManager.Instance;

        FirePoint = firePoint;
        VFX = vfx;
        TargetLayer = targetLayer;
        Force = force;
        Damage = damage;
    }
}