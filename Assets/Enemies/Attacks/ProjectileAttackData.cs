using UnityEngine;

namespace EmeWillem
{
    //[CreateAssetMenu(fileName = "Projectile AttackingState_EnteredAttackingState Data", menuName = "Scriptable Object/Data/AttackingState_EnteredAttackingState Data/Projectile")]
    //public class ProjectileAttackData : BaseAttackData
    //{
    //    [Header("PROJECTILE VARIABLES")]
    //    [SerializeField] private VFX _VFX;
    //    [SerializeField] private float _force;
    //    [SerializeField] private float _damage;

    //    private ProjectileData _projectileData;

    //    public void Init(Transform firePoint, LayerMask targetLayer)
    //    {
    //        _projectileData = new ProjectileData(firePoint, _VFX, targetLayer, _force, _damage);
    //    }

    //    public override void AttackingState_EnteredAttackingState(Transform target)
    //    {
    //        VFX projectileVFX = _projectileData.VFXManager.AddStaticVFX(_projectileData.VFX, _projectileData.FirePoint.position, _projectileData.FirePoint.rotation);
    //        projectileVFX.GetComponent<Projectile>().Init(_projectileData, target, projectileVFX);
    //    }
    //}
}