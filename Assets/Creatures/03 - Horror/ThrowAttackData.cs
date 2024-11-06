namespace EmeWillem
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Throw Attack Data", menuName = "Scriptable Object/Data/Attack Data/Throw")]
    public class ThrowAttackData : BaseAttackData
    {
        [Header("PROJECTILE VARIABLES")]
        [SerializeField] private VFX _VFX;
        [SerializeField] private float _force;
        [SerializeField] private float _damage;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _stopDuration;

        private ProjectileData _projectileData;

        public void Init(Transform firePoint, LayerMask targetLayer)
        {
            _projectileData = new ProjectileData(firePoint, _VFX, targetLayer, _force, _damage);
        }

        public override void Attack(Transform target)
        {
            VFX projectileVFX = _projectileData.VFXManager.AddStaticVFX(_projectileData.VFX, _projectileData.FirePoint.position, _projectileData.FirePoint.rotation);
            projectileVFX.GetComponent<ReturningProjectile>().Init(_projectileData, projectileVFX, _stopDuration, _maxDistance);
        }
    }
}