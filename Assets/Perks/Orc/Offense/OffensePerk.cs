//namespace EmeWillem
//{
//    using System.Collections.Generic;
//    using UnityEngine;

//    [CreateAssetMenu(fileName = "Offense Perk", menuName = "Scriptable Object/Perks/Passive Perk/Offense")]
//    public class OffensePerk : PassivePerk
//    {
//        [Header("QUAKE DAMAGE")]
//        [SerializeField] private VFX _quakeVFX;
//        [SerializeField] private float _quakeRadius = 1f;
//        [SerializeField] private float _quakeDamagePercentage = 30f;
//        [SerializeField] private bool damageInitialTarget = true;
//        private Explosion _currentExplosion;

//        [Header("QUAKE BUFF")]
//        [SerializeField] private VFX _quakeBuffVFX;
//        [SerializeField] private float _quakeBuffDuration = 3f;
//        [SerializeField] private int _quakeBuffTargetRequirement = 3;
//        [SerializeField] private float _quakeBuffDamageIncrease = 0.3f;
//        private VFX _currentQuakeBuffVFX;
//        private float _quakeBuffTimer;

//        [Header("GRIT")]
//        [SerializeField] private VFX _gritVFX;
//        [SerializeField] private float _maximumInactivity = 3f;
//        [SerializeField] private float _maxAttackDamageBonus = 0.3f;
//        [SerializeField] private float _maxCriticalChanceBonus = 30f;
//        [SerializeField] private float _maxCriticalMultiplierBonus = 0.5f;
//        private VFXEmission _currentGritVFXEmission;
//        private VFX _currentGritVFX;
//        private float _gritValue;
//        private float _inactiveTimer;

//        public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
//        {
//            statChanges = new()
//        {
//            { Stat.AttackDamageModifier, 0 },
//            { Stat.CriticalChance, 0 },
//            { Stat.CriticalMultiplier, 0 }
//        };

//            base.Init(playerObject, perks, statChanges);

//            ResetQuakeBuff();
//            ResetGrit();
//        }

//        public override void Activate()
//        {
//            _AttackHandler.SuccessfulAttack += OffensePerk_SuccessfulAttack;
//        }

//        public override void Deactivate()
//        {
//            _AttackHandler.SuccessfulAttack -= OffensePerk_SuccessfulAttack;
//            ResetQuakeBuff();
//            ResetGrit();
//        }

//        public override void FixedTick(float delta)
//        {
//            HandleQuakeBuffTick(delta);
//            HandleGritTick(delta);
//        }

//        private void OffensePerk_SuccessfulAttack(Collider hit, int colliders, float damage, bool crit)
//        {
//            HandleQuakeEffect(hit, damage);
//            HandleGritEffect(colliders, damage);
//        }

//        private void HandleQuakeEffect(Collider hit, float damage)
//        {
//            if (_quakeVFX != null)
//            {
//                Transform transform = hit.TryGetComponent(out LockTarget lockTarget) ? lockTarget.Center : hit.transform;
//                VFX quakeVFX = _VFXManager.AddMovingVFX(_quakeVFX, transform, 1f);

//                if (damageInitialTarget)
//                {
//                    hit = null;
//                }

//                float finalDamage = damage / 100 * _quakeDamagePercentage;

//                _currentExplosion = quakeVFX.GetComponent<Explosion>();
//                _currentExplosion.HitsDetected += OffensePerk_HitsDetected;
//                _currentExplosion.InitExplosion(_quakeRadius, finalDamage, _TargetLayer, hit);
//            }
//        }

//        private void HandleQuakeBuffTick(float delta)
//        {
//            if (_currentQuakeBuffVFX != null)
//            {
//                _quakeBuffTimer += delta;
//                if (_quakeBuffTimer > _quakeBuffDuration)
//                {
//                    ResetQuakeBuff();
//                }
//            }
//        }

//        private void OffensePerk_HitsDetected(int hits)
//        {
//            if (_quakeBuffVFX != null && hits >= _quakeBuffTargetRequirement)
//            {
//                _quakeBuffTimer = 0;
//                _StatTracker.IncrementStat(Stat.AttackDamageModifier, _quakeBuffDamageIncrease);

//                if (_currentQuakeBuffVFX == null)
//                {
//                    _currentQuakeBuffVFX = _VFXManager.AddMovingVFX(_quakeBuffVFX, _PlayerTransform);
//                }
//            }

//            _currentExplosion.HitsDetected -= OffensePerk_HitsDetected;
//            _currentExplosion = null;
//        }

//        private void HandleGritEffect(int colliders, float damage)
//        {
//            if (_gritVFX != null)
//            {
//                if (colliders == 1)
//                {
//                    _gritValue = Mathf.Clamp(_gritValue + damage, 0f, 100f);
//                    _inactiveTimer = 0f;
//                    ApplyGritBonuses();
//                }
//                else
//                {
//                    ResetGrit();
//                }
//            }
//        }

//        private void HandleGritTick(float delta)
//        {
//            if (_gritVFX != null)
//            {
//                _inactiveTimer += delta;
//                if (_inactiveTimer > _maximumInactivity)
//                {
//                    ResetGrit();
//                }
//            }
//        }

//        private void ApplyGritBonuses()
//        {
//            float gritPercent = _gritValue / 100f;

//            float oldAttackDamageBonus = _StatTracker.GetStatChange(Stat.AttackDamageModifier);
//            float oldCriticalChanceBonus = _StatTracker.GetStatChange(Stat.CriticalChance);
//            float oldCriticalMultiplierBonus = _StatTracker.GetStatChange(Stat.CriticalMultiplier);

//            float newAttackDamageBonus = _maxAttackDamageBonus * gritPercent;
//            float newCriticalChanceBonus = _maxCriticalChanceBonus * gritPercent;
//            float newCriticalMultiplierBonus = _maxCriticalMultiplierBonus * gritPercent;

//            _StatTracker.IncrementStat(Stat.AttackDamageModifier, newAttackDamageBonus - oldAttackDamageBonus);
//            _StatTracker.IncrementStat(Stat.CriticalChance, newCriticalChanceBonus - oldCriticalChanceBonus);
//            _StatTracker.IncrementStat(Stat.CriticalMultiplier, newCriticalMultiplierBonus - oldCriticalMultiplierBonus);

//            if (_currentGritVFX != null)
//            {
//                _currentGritVFXEmission.FixedTick(gritPercent * 25);
//            }
//            else
//            {
//                _currentGritVFX = _VFXManager.AddMovingVFX(_gritVFX, _PlayerTransform);
//                _currentGritVFXEmission = _currentGritVFX.GetComponent<VFXEmission>();
//                _currentGritVFXEmission.Init(gritPercent * 25);
//            }
//        }

//        private void ResetQuakeBuff()
//        {
//            _quakeBuffTimer = 0;
//            _StatTracker.ResetStatChanges();

//            if (_currentQuakeBuffVFX != null)
//            {
//                _VFXManager.RemoveVFX(_currentQuakeBuffVFX);
//                _currentQuakeBuffVFX = null;
//            }
//        }

//        private void ResetGrit()
//        {
//            _gritValue = 0f;
//            _inactiveTimer = 0f;
//            _StatTracker.ResetStatChanges();

//            if (_currentGritVFX != null)
//            {
//                _VFXManager.RemoveVFX(_currentGritVFX, 1);
//                _currentGritVFXEmission = null;
//                _currentGritVFX = null;
//            }
//        }
//    }
//}