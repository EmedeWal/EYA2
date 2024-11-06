namespace EmeWillem
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Mobility Perk", menuName = "Scriptable Object/Perks/Passive Perk/Mobility")]
    public class MobilityPerk : PassivePerk
    {
        private bool _isMoving;

        [Header("ICE CLOUDS")]
        [SerializeField] private VFX _iceCloudVFX;
        [SerializeField] private float _iceCloudRadius = 5f;
        [SerializeField] private float _iceCloudCooldown = 5f;
        private float _iceCloudTimer;

        [Header("SPECTRAL STRIDE")]
        [SerializeField] private VFX _movementVFX;
        [SerializeField] private float _buffEmissionRate = 5f;
        [SerializeField] private float _evasionChanceIncrease = 30f;
        [SerializeField] private float _manaRegenIncrease = 1f;
        private VFXEmission _movementVFXEmission;
        private VFX _currentMovementVFX;

        [Header("MOMENTUM SYSTEM")]
        [SerializeField] private VFX _momentumVFX;
        [SerializeField] private float _momentumIncreaseRate = 20f;
        [SerializeField] private float _momentumDecreaseRate = 30f;
        [SerializeField] private float _maxMovementSpeedBonus = 0.3f;
        [SerializeField] private float _maxAttackDamageBonus = 0.3f;
        private VFXEmission _momentumVFXEmission;
        private VFX _currentMomentumVFX;
        private float _momentum;

        public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
        {
            statChanges = new()
        {
            { Stat.ManaRegen, 0f },
            { Stat.EvasionChance, 0f },
            { Stat.MovementSpeedModifier, 0f },
            { Stat.AttackDamageModifier, 0f }
        };

            base.Init(playerObject, perks, statChanges);
        }

        public override void Activate()
        {
            if (_movementVFX != null)
            {
                EnableMovementFX(true);
            }

            _momentum = 0f;
            _iceCloudTimer = 0f;
            _isMoving = _Locomotion.Moving;

            if (_isMoving)
            {
                StartedMoving();
            }
            else
            {
                StoppedMoving();
            }
        }

        public override void Deactivate()
        {
            _StatTracker.ResetStatChanges();

            if (_currentMovementVFX != null)
            {
                EnableMovementFX(false);
            }

            if (_currentMomentumVFX != null)
            {
                EnableMomentumVFX(false, 0);
            }
        }

        public override void Tick(float delta)
        {
            bool isMoving = _Locomotion.Moving;

            HandleMovement(isMoving);

            _isMoving = isMoving;

            if (_iceCloudVFX != null)
            {
                HandleIceClouds(delta);
            }

            if (_momentumVFX != null)
            {
                HandleMomentum(delta);
            }
        }

        private void HandleMovement(bool isMoving)
        {
            if (_isMoving != isMoving)
            {
                if (isMoving)
                {
                    StartedMoving();
                }
                else
                {
                    StoppedMoving();
                }
            }
        }

        private void HandleIceClouds(float delta)
        {
            if (_isMoving)
            {
                _iceCloudTimer += delta;
                if (_iceCloudTimer >= _iceCloudCooldown)
                {
                    _iceCloudTimer = 0;

                    VFX iceCloud = _VFXManager.AddStaticVFX(_iceCloudVFX, _PlayerTransform.position, _PlayerTransform.rotation, 4f);
                    iceCloud.GetComponent<FreezingExplosion>().Init(_iceCloudRadius, _TargetLayer);

                    AudioSource source = iceCloud.GetComponent<AudioSource>();
                    _AudioSystem.PlayAudio(source, source.clip, source.volume);
                }
            }
            else
            {
                _iceCloudTimer = 0;
            }
        }

        private void HandleMomentum(float delta)
        {
            if (_momentumVFX != null)
            {
                if (_isMoving)
                {
                    _momentum = Mathf.Clamp(_momentum + _momentumIncreaseRate * delta, 0f, 100f);
                }
                else
                {
                    _momentum = Mathf.Clamp(_momentum - _momentumDecreaseRate * delta, 0f, 100f);
                }

                ApplyBonusesBasedOnMomentum();
            }
        }

        private void ApplyBonusesBasedOnMomentum()
        {
            float momentumPercent = _momentum / 100f;
            float newMovementSpeedBonus = _maxMovementSpeedBonus * momentumPercent;
            float newAttackDamageBonus = _maxAttackDamageBonus * momentumPercent;

            _StatTracker.IncrementStat(Stat.MovementSpeedModifier, newMovementSpeedBonus - _StatTracker.GetStatChange(Stat.MovementSpeedModifier));
            _StatTracker.IncrementStat(Stat.AttackDamageModifier, newAttackDamageBonus - _StatTracker.GetStatChange(Stat.AttackDamageModifier));

            if (_momentumVFXEmission != null)
            {
                _momentumVFXEmission.Tick(momentumPercent * 25);
            }
            else
            {
                EnableMomentumVFX(true, momentumPercent);
            }
        }

        private void EnableMomentumVFX(bool enable, float momentum)
        {
            if (enable)
            {
                _currentMomentumVFX = _VFXManager.AddMovingVFX(_momentumVFX, _PlayerTransform);
                _momentumVFXEmission = _currentMomentumVFX.GetComponent<VFXEmission>();
                _momentumVFXEmission.Init(momentum * 25);
            }
            else
            {
                _VFXManager.RemoveVFX(_currentMomentumVFX, 1);
                _momentumVFXEmission = null;
                _currentMomentumVFX = null;
            }
        }

        private void EnableMovementFX(bool enable)
        {
            if (enable)
            {
                _currentMovementVFX = _VFXManager.AddMovingVFX(_movementVFX, _PlayerTransform);
                _movementVFXEmission = _currentMovementVFX.GetComponent<VFXEmission>();
                _movementVFXEmission.Init(0);
            }
            else if (_currentMovementVFX != null)
            {
                _VFXManager.RemoveVFX(_currentMovementVFX, 1f);
            }
        }

        private void StartedMoving()
        {
            if (_movementVFXEmission != null)
            {
                _movementVFXEmission.Tick(_buffEmissionRate);
            }

            _StatTracker.IncrementStat(Stat.ManaRegen, _manaRegenIncrease);
            _StatTracker.IncrementStat(Stat.EvasionChance, _evasionChanceIncrease);
        }

        private void StoppedMoving()
        {
            if (_movementVFXEmission != null)
            {
                _movementVFXEmission.Tick(0);
            }

            _StatTracker.IncrementStat(Stat.ManaRegen, -_StatTracker.GetStatChange(Stat.ManaRegen));
            _StatTracker.IncrementStat(Stat.EvasionChance, -_StatTracker.GetStatChange(Stat.EvasionChance));
        }
    }
}