using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mobility Perk", menuName = "Scriptable Object/Perks/Passive Perk/Mobility")]
public class MobilityPerk : PerkData
{
    private PlayerLocomotion _playerLocomotion;
    private PlayerAttackHandler _playerAttackHandler;
    private Mana _playerMana;

    [Header("ICE CLOUDS")]
    [SerializeField] private VFX _iceCloudPrefab;
    [SerializeField] private float _iceCloudRadius = 5f;
    [SerializeField] private float _iceCloudCooldown = 5f;
    private LayerMask _targetLayers;
    private float _iceCloudTimer;

    [Header("SPECTRAL STRIDE")]
    [SerializeField] private VFX _movementBuff;
    [SerializeField] private float _buffEmissionRate = 5f;
    [SerializeField] private float _evasionChanceIncrease = 30f;
    [SerializeField] private float _manaRegenIncrease = 1f;
    private VFXEmission _currentBuffEmission;
    private VFX _currentBuff;

    [Header("MOMENTUM SYSTEM")]
    [SerializeField] private VFX _momentumEffectPrefab;
    [SerializeField] private float _momentumIncreaseRate = 20f;
    [SerializeField] private float _momentumDecreaseRate = 30f;
    [SerializeField] private float _maxMovementSpeedBonus = 0.3f;
    [SerializeField] private float _maxAttackDamageBonus = 0.3f;
    private VFXEmission _currentParticleEmissionModifier;
    private VFX _currentMomentumEffect;
    private float _previousMovementSpeedBonus; 
    private float _previousAttackDamageBonus;
    private float _momentum;

    private bool _isMoving;

    private VFXManager _VFXManager;

    public override void Init(GameObject playerObject, List<PerkData> perks = null)
    {
        base.Init(playerObject, perks);

        for (int i = perks.Count - 1; i >= 0; i--)
        {
            PerkData perk = perks[i];
            if (perk.GetType() == GetType())
            {
                perk.Deactivate();
                perks.RemoveAt(i);
            }
        }

        _playerLocomotion = _PlayerObject.GetComponent<PlayerLocomotion>();
        _playerAttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();
        _playerMana = _PlayerObject.GetComponent<Mana>();

        _targetLayers = LayerMask.GetMask("DamageCollider");

        _VFXManager = VFXManager.Instance;

        ResetPerkState();
    }

    public override void Activate()
    {
        EnableBuffVFX(true);
    }

    public override void Deactivate()
    {
        if (_isMoving)
        {
            StoppedMoving();
        }

        ResetPerkState();
    }

    public override void Tick(float delta)
    {
        bool isMoving = _playerLocomotion.Moving;

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

        _isMoving = isMoving;

        if (_iceCloudPrefab != null)
        {
            if (_isMoving)
            {
                _iceCloudTimer += delta;

                if (_iceCloudTimer >= _iceCloudCooldown)
                {
                    _iceCloudTimer = 0;
                    VFX iceCloud = Instantiate(_iceCloudPrefab, _PlayerTransform);
                    _VFXManager.AddVFX(iceCloud, iceCloud.transform, true, 4f);
                    AudioSource source = iceCloud.GetComponent<AudioSource>();
                    AudioSystem.Instance.PlayAudioClip(source, source.clip, source.volume);
                    iceCloud.GetComponent<FreezingExplosion>().Init(_iceCloudRadius, _targetLayers);
                }
            }
            else
            {
                _iceCloudTimer = 0;
            }
        }

        if (_momentumEffectPrefab == null) return;

        if (isMoving)
        {
            IncreaseMomentum(delta);
        }
        else
        {
            DecreaseMomentum(delta);
        }

        ApplyBonusesBasedOnMomentum();
    }

    private void IncreaseMomentum(float delta)
    {
        _momentum = Mathf.Clamp(_momentum + _momentumIncreaseRate * delta, 0f, 100f);
    }

    private void DecreaseMomentum(float delta)
    {
        _momentum = Mathf.Clamp(_momentum - _momentumDecreaseRate * delta, 0f, 100f);
    }

    private void ApplyBonusesBasedOnMomentum()
    {
        float momentumPercent = _momentum / 100f;

        float newMovementSpeedBonus = _maxMovementSpeedBonus * momentumPercent;
        float newAttackDamageBonus = _maxAttackDamageBonus * momentumPercent;

        float movementSpeedBonusChange = newMovementSpeedBonus - _previousMovementSpeedBonus;
        float attackDamageBonusChange = newAttackDamageBonus - _previousAttackDamageBonus;

        _PlayerStats.IncrementStat(Stat.MovementSpeedModifier, movementSpeedBonusChange);
        _PlayerStats.IncrementStat(Stat.AttackDamageModifier, attackDamageBonusChange);

        _previousMovementSpeedBonus = newMovementSpeedBonus;
        _previousAttackDamageBonus = newAttackDamageBonus;

        if (_currentMomentumEffect != null)
        {
            _currentParticleEmissionModifier.Tick(momentumPercent * 25);
        }
        else
        {
            _currentMomentumEffect = Instantiate(_momentumEffectPrefab, _PlayerTransform);
            _VFXManager.AddVFX(_currentMomentumEffect, _PlayerTransform);

            _currentParticleEmissionModifier = _currentMomentumEffect.GetComponent<VFXEmission>();
            _currentParticleEmissionModifier.Init(momentumPercent * 25);
        }
    }

    private void EnableBuffVFX(bool enable)
    {
        if (_movementBuff == null) return;

        if (enable)
        {
            _currentBuff = Instantiate(_movementBuff, _PlayerTransform);
            _VFXManager.AddVFX(_currentBuff, _PlayerTransform);

            _currentBuffEmission = _currentBuff.GetComponent<VFXEmission>();
            _currentBuffEmission.Init(0);
        }
        else if (_currentBuff != null) 
        {
            _VFXManager.RemoveVFX(_currentBuff, 1f);
        }
    }

    private void ResetPerkState()
    {
        _isMoving = false;

        _momentum = 0f;
        _iceCloudTimer = 0f;

        _PlayerStats.IncrementStat(Stat.MovementSpeedModifier, -_previousMovementSpeedBonus);
        _PlayerStats.IncrementStat(Stat.AttackDamageModifier, -_previousAttackDamageBonus);

        _previousMovementSpeedBonus = 0f;
        _previousAttackDamageBonus = 0f;

        EnableBuffVFX(false);

        if (_currentMomentumEffect != null)
        {
            _VFXManager.RemoveVFX(_currentMomentumEffect, 1);
            _currentParticleEmissionModifier = null;
            _currentMomentumEffect = null;
        }
    }

    private void StartedMoving()
    {
        if (_currentBuffEmission != null)
        {
            _currentBuffEmission.Tick(_buffEmissionRate);
        }

        _PlayerStats.IncrementStat(Stat.ManaRegen, _manaRegenIncrease);
        _PlayerStats.IncrementStat(Stat.EvasionChance, _evasionChanceIncrease);
    }

    private void StoppedMoving()
    {
        if (_currentBuffEmission != null)
        {
            _currentBuffEmission.Tick(0);
        }

        _PlayerStats.IncrementStat(Stat.ManaRegen, -_manaRegenIncrease);
        _PlayerStats.IncrementStat(Stat.EvasionChance, -_evasionChanceIncrease);
    }
}
