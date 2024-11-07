//namespace EmeWillem
//{
//    using System.Collections.Generic;
//    using UnityEngine;

//    [CreateAssetMenu(fileName = "Defense Perk", menuName = "Scriptable Object/Perks/Passive Perk/Defense")]
//    public class DefensePerk : PassivePerk
//    {
//        [Header("STONE WALL")]
//        [SerializeField] private VFX _buffVFX;
//        [SerializeField] private float _damageReductionIncrease = 10f;
//        [SerializeField] private float _healthRegenIncrease = 3f;
//        private VFX _currentBuffVFX;
//        private bool _isMoving;

//        [Header("UNYIELDING")]
//        [SerializeField] private bool _enableResurrection = true;
//        [SerializeField] private float _resurrectionHealThreshold = 50f;
//        [SerializeField] private float _cooldown = 120f;
//        private float _cooldownTimer;
//        private bool _onCooldown;

//        public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
//        {
//            statChanges = new()
//        {
//            { Stat.DamageReduction, 0 },
//            {Stat.HealthRegen, 0 }
//        };

//            base.Init(playerObject, perks, statChanges);

//            _cooldownTimer = 0;
//            _onCooldown = false;
//        }

//        public override void Activate()
//        {
//            _Health.Resurrected += DefensePerk_Resurrected;

//            bool resurrection;

//            if (_onCooldown)
//            {
//                resurrection = false;
//            }
//            else
//            {
//                resurrection = _enableResurrection;
//            }

//            _Health.EnableResurrection(resurrection);

//            _isMoving = _Locomotion.Moving;

//            if (_isMoving)
//            {
//                StartedMoving();
//            }
//            else
//            {
//                StoppedMoving();
//            }

//            EnableBuffVFX(true);
//        }

//        public override void Deactivate()
//        {
//            _Health.Resurrected -= DefensePerk_Resurrected;
//            _Health.EnableResurrection(false);

//            _StatTracker.ResetStatChanges();

//            EnableBuffVFX(false);
//        }

//        public override void Tick(float delta)
//        {
//            bool isMoving = _Locomotion.Moving;

//            if (_isMoving != isMoving)
//            {
//                if (isMoving)
//                {
//                    StartedMoving();
//                }
//                else
//                {
//                    StoppedMoving();
//                }
//            }

//            _isMoving = isMoving;

//            if (_onCooldown)
//            {
//                _cooldownTimer += delta;

//                if (_cooldownTimer > _cooldown)
//                {
//                    _Health.EnableResurrection(true);
//                    _onCooldown = false;
//                    _cooldownTimer = 0;
//                }
//            }
//        }

//        private void EnableBuffVFX(bool enable)
//        {
//            if (_buffVFX != null)
//            {
//                if (enable)
//                {
//                    _currentBuffVFX = _VFXManager.AddMovingVFX(_buffVFX, _PlayerTransform);
//                }
//                else
//                {
//                    _VFXManager.RemoveVFX(_currentBuffVFX, 1f);
//                }
//            }
//        }

//        private void StartedMoving()
//        {
//            if (_currentBuffVFX != null)
//            {
//                _currentBuffVFX.Deactivate();
//            }

//            _StatTracker.ResetStatChanges();
//        }

//        private void StoppedMoving()
//        {
//            if (_currentBuffVFX != null)
//            {
//                _currentBuffVFX.Activate();
//            }

//            _StatTracker.IncrementStat(Stat.HealthRegen, _healthRegenIncrease);
//            _StatTracker.IncrementStat(Stat.DamageReduction, _damageReductionIncrease);
//        }

//        private void DefensePerk_Resurrected()
//        {
//            float targetHealth = _PlayerStats.GetBaseStat(Stat.MaxHealth) / 100 * _resurrectionHealThreshold;
//            float currentHeath = _Health.CurrentHealth;
//            float healAmount = targetHealth - currentHeath;

//            if (targetHealth > currentHeath)
//            {
//                //_Health.Heal(healAmount);
//            }

//            _Health.EnableResurrection(false);
//            _onCooldown = true;
//            _cooldownTimer = 0;
//        }
//    }

//}