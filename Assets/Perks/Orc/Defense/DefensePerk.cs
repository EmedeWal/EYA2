using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defense Perk", menuName = "Scriptable Object/Perks/Passive Perk/Defense")]
public class DefensePerk : PerkData
{
    private PlayerLocomotion _playerLocomotion;
    private Health _playerHealth;

    [Header("STONE WALL")]
    [SerializeField] private VFX _buff;
    [SerializeField] private float _buffEmissionRate = 5f;
    [SerializeField] private float _damageReductionIncrease = 10f;
    [SerializeField] private float _healthRegenIncrease = 3f;
    private VFXEmission _buffEmission;
    private VFX _currentBuff;
    private bool _isMoving;

    [Header("UNYIELDING")]
    [SerializeField] private bool _enableResurrection = true;
    [SerializeField] private float _resurrectionHealThreshold = 50f;
    [SerializeField] private float _cooldown = 120f;
    private float _cooldownTimer;
    private bool _onCooldown;

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
        _playerHealth = _PlayerObject.GetComponent<Health>();

        _VFXManager = VFXManager.Instance;

        ResetPerkState();
    }

    public override void Activate()
    {
        _playerHealth.Resurrected += DefensePerk_Resurrected;

        bool resurrection;

        if (_onCooldown)
        {
            resurrection = false;
        }
        else
        {
            resurrection = _enableResurrection;
        }

        _playerHealth.EnableResurrection(resurrection);

        EnableBuffVFX(true);

        if (_playerLocomotion.Moving)
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
        _playerHealth.Resurrected -= DefensePerk_Resurrected;
        _playerHealth.EnableResurrection(false);

        if (!_isMoving)
        {
            StartedMoving();
        }

        EnableBuffVFX(false);
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

        if (!_onCooldown) return;

        _cooldownTimer += delta;

        if (_cooldownTimer > _cooldown)
        {
            _playerHealth.EnableResurrection(true);
            _onCooldown = false;
            _cooldownTimer = 0;
        }
    }

    private void EnableBuffVFX(bool enable)
    {
        if (_buff == null) return;

        if (enable)
        {
            _currentBuff = Instantiate(_buff, _PlayerTransform);
            _VFXManager.AddVFX(_currentBuff, _PlayerTransform);

            _buffEmission = _currentBuff.GetComponent<VFXEmission>();
            _buffEmission.Init(0);
        }
        else if (_currentBuff != null)
        {
            _VFXManager.RemoveVFX(_currentBuff, 1f);
        }
    }

    private void ResetPerkState()
    {
        _cooldownTimer = 0;
        _onCooldown = false;
        EnableBuffVFX(false);
    }

    private void StartedMoving()
    {
        if (_buffEmission != null)
        {
            _buffEmission.Tick(0);
        }

        _PlayerStats.IncrementStat(Stat.HealthRegen, -_healthRegenIncrease);
        _PlayerStats.IncrementStat(Stat.DamageReduction, -_damageReductionIncrease);
    }

    private void StoppedMoving()
    {
        if (_buffEmission != null)
        {
            _buffEmission.Tick(_buffEmissionRate);
        }

        _PlayerStats.IncrementStat(Stat.HealthRegen, _healthRegenIncrease);
        _PlayerStats.IncrementStat(Stat.DamageReduction, _damageReductionIncrease);
    }

    private void DefensePerk_Resurrected()
    {
        float targetHealth = _PlayerStats.GetBaseStat(Stat.MaxHealth) / 100 * _resurrectionHealThreshold;
        float currentHeath = _playerHealth.CurrentValue;
        float healAmount = targetHealth - currentHeath;

        if (targetHealth > currentHeath)
        {
            _playerHealth.Heal(healAmount);
        }

        _playerHealth.EnableResurrection(false);
        _onCooldown = true;
        _cooldownTimer = 0;
    }
}
