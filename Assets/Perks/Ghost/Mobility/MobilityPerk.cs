using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mobility Perk", menuName = "Scriptable Object/Perks/Passive Perk/Mobility")]
public class MobilityPerk : PerkData
{
    private StatTracker _statTracker;
    private PlayerLocomotion _playerLocomotion;
    private PlayerAttackHandler _playerAttackHandler;
    private Mana _playerMana;
    private bool _isMoving;

    [Header("ICE CLOUDS")]
    [SerializeField] private VFX _iceCloudVFX;
    [SerializeField] private float _iceCloudRadius = 5f;
    [SerializeField] private float _iceCloudCooldown = 5f;
    private LayerMask _targetLayers;
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

    private AudioSystem _audioSystem;
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

        Dictionary<Stat, float> statChanges = new()
        {
            { Stat.ManaRegen, 0f },
            { Stat.EvasionChance, 0f },
            { Stat.MovementSpeedModifier, 0f },
            { Stat.AttackDamageModifier, 0f }
        };

        _statTracker = new StatTracker(statChanges, _PlayerStats);
        _playerLocomotion = _PlayerObject.GetComponent<PlayerLocomotion>();
        _playerAttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();
        _playerMana = _PlayerObject.GetComponent<Mana>();
        _targetLayers = LayerMask.GetMask("DamageCollider");
        _audioSystem = AudioSystem.Instance;
        _VFXManager = VFXManager.Instance;
    }

    public override void Activate()
    {
        _iceCloudTimer = 0f;
        _momentum = 0f;

        _isMoving = _playerLocomotion.Moving;

        if (_isMoving)
        {
            StartedMoving();
        }
        else
        {
            StoppedMoving();
        }

        if (_movementVFX != null)
        {
            EnableMovementFX(true);
        }
    }

    public override void Deactivate()
    {
        _statTracker.ResetStatChanges();

        if (_currentMovementVFX != null)
        {
            EnableMovementFX(false);
        }

        if (_currentMomentumVFX != null)
        {
            _VFXManager.RemoveVFX(_currentMomentumVFX, 1);
            _momentumVFXEmission = null;
            _currentMomentumVFX = null;
        }
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

        if (_iceCloudVFX != null)
        {
            if (_isMoving)
            {
                _iceCloudTimer += delta;
                if (_iceCloudTimer >= _iceCloudCooldown)
                {
                    _iceCloudTimer = 0;

                    VFX iceCloud = _VFXManager.AddStaticVFX(_iceCloudVFX, _PlayerTransform.position, _PlayerTransform.rotation, 4f);
                    iceCloud.GetComponent<FreezingExplosion>().Init(_iceCloudRadius, _targetLayers);

                    AudioSource source = iceCloud.GetComponent<AudioSource>();
                    AudioSystem.Instance.PlayAudio(source, source.clip, source.volume);
                }
            }
            else
            {
                _iceCloudTimer = 0;
            }
        }

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

        _statTracker.IncrementStat(Stat.MovementSpeedModifier, newMovementSpeedBonus - _statTracker.GetStatChange(Stat.MovementSpeedModifier));
        _statTracker.IncrementStat(Stat.AttackDamageModifier, newAttackDamageBonus - _statTracker.GetStatChange(Stat.AttackDamageModifier));

        if (_momentumVFXEmission != null)
        {
            _momentumVFXEmission.Tick(momentumPercent * 25);
        }
        else
        {
            _currentMomentumVFX = _VFXManager.AddMovingVFX(_momentumVFX, _PlayerTransform);
            _momentumVFXEmission = _currentMomentumVFX.GetComponent<VFXEmission>();
            _momentumVFXEmission.Init(momentumPercent * 25);
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

        _statTracker.IncrementStat(Stat.ManaRegen, _manaRegenIncrease);
        _statTracker.IncrementStat(Stat.EvasionChance, _evasionChanceIncrease);
    }

    private void StoppedMoving()
    {
        if (_movementVFXEmission != null)
        {
            _movementVFXEmission.Tick(0);
        }

        _statTracker.IncrementStat(Stat.ManaRegen, -_statTracker.GetStatChange(Stat.ManaRegen));
        _statTracker.IncrementStat(Stat.EvasionChance, -_statTracker.GetStatChange(Stat.EvasionChance));
    }
}