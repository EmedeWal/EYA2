using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Attack Data", menuName = "Scriptable Object/Data/Attack Data/Projectile")]
public class ProjectileAttackData : AttackData
{
    [Header("PROJECTILE VARIABLES")]
    [SerializeField] private VFX _VFX;
    [SerializeField] private float _force;
    [SerializeField] private float _damage;

    private ProjectileData _projectileData;

    public void Init(Transform firePoint, LayerMask targetLayer)
    {
        _projectileData = new ProjectileData(firePoint, _VFX, targetLayer, _force, _damage);
    }

    public override void Attack(Transform target)
    {
        VFX projectileVFX = _projectileData.VFXManager.AddStaticVFX(_projectileData.VFX, _projectileData.FirePoint.position, _projectileData.FirePoint.rotation);
        projectileVFX.GetComponent<Projectile>().Init(_projectileData, target, projectileVFX);
    }
}