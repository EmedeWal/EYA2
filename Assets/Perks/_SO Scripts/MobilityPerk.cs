using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobilityPerk", menuName = "Scriptable Object/Perks/Passive Perk/Mobility")]
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
    [SerializeField] private float _evasionChanceIncrease = 30f;
    [SerializeField] private float _manaRegenIncrease = 1f;

    [Header("MOMENTUM SYSTEM")]
    [SerializeField] private VFX _momentumEffectPrefab;
    [SerializeField] private float _momentumIncreaseRate = 20f;
    [SerializeField] private float _momentumDecreaseRate = 30f;
    [SerializeField] private float _maxMovementSpeedBonus = 0.3f;
    [SerializeField] private float _maxAttackDamageBonus = 0.3f;
    private ParticleEmissionModifier _currentParticleEmissionModifier;
    private VFX _currentMomentumEffect;
    private float _previousMovementSpeedBonus; 
    private float _previousAttackDamageBonus;
    private float _momentum;

    private bool _isMoving;

    public override void Init(List<PerkData> perks, GameObject playerObject)
    {
        base.Init(perks, playerObject);

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

        _targetLayers = LayerMask.NameToLayer("Damage Collider");

        ResetPerkState();
    }

    public override void Tick(float delta)
    {
        bool isMoving = _playerLocomotion.IsMoving;

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
            _iceCloudTimer += delta;

            if (_iceCloudTimer >= _iceCloudCooldown)
            {
                _iceCloudTimer = 0;
                VFX iceCloud = Instantiate(_iceCloudPrefab, _PlayerTransform);
                VFXManager.Instance.AddVFX(iceCloud, iceCloud.transform, true, 3f);
                iceCloud.GetComponent<FreezingExplosion>().Init(_iceCloudRadius, _targetLayers);
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

    public override void Deactivate()
    {
        if (_isMoving)
        {
            StoppedMoving();
        }

        ResetPerkState();
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
            VFXManager.Instance.AddVFX(_currentMomentumEffect, _PlayerTransform);

            _currentParticleEmissionModifier = _currentMomentumEffect.GetComponent<ParticleEmissionModifier>();
            _currentParticleEmissionModifier.Init(momentumPercent * 25);
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

        if (_currentMomentumEffect != null)
        {
            VFXManager.Instance.RemoveVFX(_currentMomentumEffect, 1);
            _currentParticleEmissionModifier = null;
            _currentMomentumEffect = null;
        }
    }

    private void StartedMoving()
    {
        _PlayerStats.IncrementStat(Stat.ManaRegen, _manaRegenIncrease);
        _PlayerStats.IncrementStat(Stat.EvasionChance, _evasionChanceIncrease);
    }

    private void StoppedMoving()
    {
        _PlayerStats.IncrementStat(Stat.ManaRegen, -_manaRegenIncrease);
        _PlayerStats.IncrementStat(Stat.EvasionChance, -_evasionChanceIncrease);
    }
}
