using UnityEngine;

namespace EmeWillem
{
    [CreateAssetMenu(fileName = "Grab Attack Data", menuName = "Scriptable Object/Data/Attack Data/Grab")]
    public class GrabAttackData : BaseAttackData
    {
        [Header("GRAB")]
        [SerializeField] private VFX _grabVFX;
        [SerializeField] private float _radius;
        [SerializeField] private float _damage;
        [SerializeField] private float _duration;
        private PlayerStats _playerStats;
        private VFXManager _VFXManager;
        private Transform _grabOrigin;
        private Health _creatureHealth;
        private LayerMask _targetLayer;

        public void Init(PlayerStats playerStats, Transform grabOrigin, Health creatureHealth, LayerMask targetLayer)
        {
            _VFXManager = VFXManager.Instance;

            _creatureHealth = creatureHealth;
            _targetLayer = targetLayer;
            _playerStats = playerStats;
            _grabOrigin = grabOrigin;
        }

        public override void Attack(Transform target)
        {
            VFX currentVFX = _VFXManager.AddStaticVFX(_grabVFX, _grabOrigin.position, _grabOrigin.rotation);
            currentVFX.GetComponent<HorrorGrab>().InitHorrorGrab(_radius, _damage, _duration, _targetLayer, currentVFX, _creatureHealth, _playerStats);
        }
    }
}