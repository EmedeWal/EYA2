//namespace EmeWillem
//{
//    using System.Collections.Generic;
//    using UnityEngine;

//    [CreateAssetMenu(fileName = "EvasionPerk", menuName = "Scriptable Object/Perks/Passive Perk/Evasion")]
//    public class EvasionPerk : PassivePerk
//    {
//        [Header("VARIABLES")]
//        [SerializeField] private float _maxEvasionChanceIncrease = 25f;
//        [SerializeField] private float _completionTime = 10f;
//        private float _evasionChanceIncrement;
//        private float _evasionTimer;

//        [Header("SHIELD")]
//        [SerializeField] private VFX _shieldVFX;
//        [SerializeField] private int _shieldCount = 1;
//        [SerializeField] private bool _damageReflection = false;
//        [SerializeField] private bool _manaRestoration = false;
//        private VFX _currentShieldVFX;
//        private int _currentShieldCount;
//        private float _shieldTimer;
//        private bool _shielded = false;

//        [Header("EXPLOSION")]
//        [SerializeField] private VFX _shieldExplosionVFX;
//        [SerializeField] private float _radius = 4f;
//        [SerializeField] private float _damage = 50f;

//        public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
//        {
//            statChanges = new()
//        {
//            { Stat.EvasionChance, 0 }
//        };

//            base.Init(playerObject, perks, statChanges);

//            _evasionChanceIncrement = _maxEvasionChanceIncrease / _completionTime;
//            _evasionTimer = 0;

//            _currentShieldCount = _shieldCount;
//            _shieldTimer = _completionTime;
//        }

//        public override void Activate()
//        {
//            _Health.HealthRemoved += EvasionPerk_ValueRemoved;
//            StartShieldGFX();
//        }

//        public override void Deactivate()
//        {
//            _Health.HealthRemoved -= EvasionPerk_ValueRemoved;
//            ResetPerkState();
//        }

//        public override void FixedTick(float delta)
//        {
//            if (_StatTracker.GetStatChange(Stat.EvasionChance) < _maxEvasionChanceIncrease)
//            {
//                IncrementEvasion(delta);
//            }

//            if (_shieldVFX != null && !_shielded)
//            {
//                HandleShielding(delta);
//            }
//        }

//        private void EvasionPerk_HitShielded(GameObject attackerObject, float damageShielded)
//        {
//            _currentShieldCount--;

//            if (_currentShieldCount <= 0)
//            {
//                EnableShield(false);
//                StartShieldGFX();
//            }

//            if (_shieldExplosionVFX != null)
//            {
//                HandleShieldExplosion();
//            }

//            if (_damageReflection)
//            {
//                //if (attackerObject.TryGetComponent(out Health attackerHealth))
//                //{
//                //    attackerHealth.TakeDamage(_PlayerObject, damageShielded * 2);
//                //}
//            }

//            if (_manaRestoration)
//            {
//                //_Mana.InflictStagger(damageShielded);
//            }
//        }

//        private void EvasionPerk_ValueRemoved(int amount)
//        {
//            ResetPerkState();
//            StartShieldGFX();
//        }

//        private void IncrementEvasion(float delta)
//        {
//            _evasionTimer += delta;
//            if (_evasionTimer >= 1f)
//            {
//                _evasionTimer = 0f;
//                _StatTracker.IncrementStat(Stat.EvasionChance, _evasionChanceIncrement);
//            }
//        }

//        private void HandleShielding(float delta)
//        {
//            _shieldTimer -= delta;
//            if (_shieldTimer <= 0)
//            {
//                EnableShield(true);
//                _shieldTimer = _completionTime;
//            }
//        }

//        private void HandleShieldExplosion()
//        {
//            VFX explosionVFX = _VFXManager.AddStaticVFX(_shieldExplosionVFX, _PlayerTransform.position, _PlayerTransform.rotation, 5f);
//            explosionVFX.GetComponent<Explosion>().InitExplosion(_radius, _damage, _TargetLayer);

//            AudioSource source = explosionVFX.GetComponent<AudioSource>();
//            AudioSystem.Instance.PlayAudio(source, source.clip, source.volume);
//        }

//        private void EnableShield(bool enabled)
//        {
//            _shielded = enabled;

//            if (enabled)
//            {
//                _Health.Shielded = true;
//                _Health.HitShielded += EvasionPerk_HitShielded;
//                _currentShieldVFX.GetComponent<VFXPlayer>().PlayVFXInChildren();

//                AudioSource source = _currentShieldVFX.GetComponent<AudioSource>();
//                AudioSystem.Instance.PlayAudio(source, source.clip, source.volume);
//            }
//            else
//            {
//                _Health.HitShielded -= EvasionPerk_HitShielded;
//                _VFXManager.RemoveVFX(_currentShieldVFX);
//                _currentShieldCount = _shieldCount;
//                _Health.Shielded = false;
//            }
//        }

//        private void StartShieldGFX()
//        {
//            if (_shieldVFX != null)
//            {
//                _currentShieldVFX = _VFXManager.AddMovingVFX(_shieldVFX, _PlayerTransform);
//            }
//        }

//        private void ResetPerkState()
//        {
//            _StatTracker.ResetStatChanges();
//            _shieldTimer = _completionTime;
//            _evasionTimer = 0;

//            if (_currentShieldVFX != null)
//            {
//                EnableShield(false);
//            }
//        }
//    }
//}