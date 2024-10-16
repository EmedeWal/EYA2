using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvasionPerk", menuName = "Scriptable Object/Perks/Passive Perk/Evasion")]
public class EvasionPerk : PerkData
{
    private StatTracker _statTracker;

    [Header("VARIABLES")]
    [SerializeField] private float _maxEvasionChanceIncrease = 25f;
    [SerializeField] private float _completionTime = 10f;
    private float _evasionChanceIncrement;
    private float _evasionTimer;

    [Header("SHIELD")]
    [SerializeField] private VFX _shieldVFX;
    [SerializeField] private int _shieldCount = 1;
    [SerializeField] private bool _damageReflection = false;
    [SerializeField] private bool _manaRestoration = false;
    private VFX _currentShieldVFX;
    private int _currentShieldCount;
    private float _shieldTimer;
    private bool _shielded = false;

    [Header("EXPLOSION")]
    [SerializeField] private VFX _shieldExplosionVFX;
    [SerializeField] private float _radius = 4f;
    [SerializeField] private float _damage = 50f;
    private LayerMask _targetLayer;

    private Health _playerHealth;
    private Mana _playerMana;

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

        _evasionChanceIncrement = _maxEvasionChanceIncrease / _completionTime;
        _evasionTimer = 0;

        Dictionary<Stat, float> statChanges = new()
        {
            { Stat.EvasionChance, 0 }
        };

        _statTracker = new StatTracker(statChanges, _PlayerStats);

        _currentShieldCount = _shieldCount;
        _shieldTimer = _completionTime;

        _targetLayer = LayerMask.NameToLayer("DamageCollider");

        _playerHealth = _PlayerObject.GetComponent<Health>();
        _playerMana = _PlayerObject.GetComponent<Mana>();

        _VFXManager = VFXManager.Instance;
    }

    public override void Activate()
    {
        _playerHealth.ValueRemoved += EvasionPerk_ValueRemoved;
        StartShieldGFX();
    }

    public override void Deactivate()
    {
        _playerHealth.ValueRemoved -= EvasionPerk_ValueRemoved;
        ResetPerkState();
    }

    public override void Tick(float delta)
    {
        if (_statTracker.GetStatChange(Stat.EvasionChance) < _maxEvasionChanceIncrease)
        {
            _evasionTimer += delta;
            if (_evasionTimer >= 1f)
            {
                _evasionTimer = 0f;
                _statTracker.IncrementStat(Stat.EvasionChance, _evasionChanceIncrement);
            }
        }

        if (_shieldVFX != null && !_shielded)
        {
            _shieldTimer -= delta;
            if (_shieldTimer <= 0)
            {
                EnableShield(true);
                _shieldTimer = _completionTime;
            }
        }
    }

    private void EvasionPerk_HitShielded(GameObject attackerObject, float damageShielded)
    {
        _currentShieldCount--;

        if (_currentShieldCount <= 0)
        {
            EnableShield(false);
            StartShieldGFX();
        }

        if (_shieldExplosionVFX != null)
        {
            VFX explosionVFX = _VFXManager.AddVFX(_shieldExplosionVFX, true, 5);
            Explosion explosion = explosionVFX.GetComponent<Explosion>();
            explosion.InitExplosion(_radius, _damage, _targetLayer);

            AudioSource source = explosionVFX.GetComponent<AudioSource>();
            AudioSystem.Instance.PlayAudio(source, source.clip, source.volume);
        }

        if (_damageReflection)
        {
            if (attackerObject.TryGetComponent(out Health attackerHealth))
            {
                attackerHealth.TakeDamage(_PlayerObject, damageShielded);
            }
        }

        if (_manaRestoration)
        {
            _playerMana.Gain(damageShielded);
        }
    }

    private void EvasionPerk_ValueRemoved()
    {
        ResetPerkState();
        StartShieldGFX();
    }

    private void EnableShield(bool enabled)
    {
        _shielded = enabled;

        if (enabled)
        {
            _playerHealth.Shielded = true;
            _playerHealth.HitShielded += EvasionPerk_HitShielded;
            _currentShieldVFX.GetComponent<VFXPlayer>().PlayVFXInChildren();

            AudioSource source = _currentShieldVFX.GetComponent<AudioSource>();
            AudioSystem.Instance.PlayAudio(source, source.clip, source.volume);
        }
        else
        {
            _playerHealth.HitShielded -= EvasionPerk_HitShielded;
            _VFXManager.RemoveVFX(_currentShieldVFX);
            _currentShieldCount = _shieldCount;
            _playerHealth.Shielded = false;
        }
    }

    private void StartShieldGFX()
    {
        if (_shieldVFX != null)
        {
            _currentShieldVFX = _VFXManager.AddVFX(_shieldVFX, false, 0, _PlayerTransform.position, _PlayerTransform.rotation, _PlayerTransform);
        }
    }

    private void ResetPerkState()
    {
        _statTracker.ResetStatChanges();
        _shieldTimer = _completionTime;
        _evasionTimer = 0;

        if (_currentShieldVFX != null)
        {
            EnableShield(false);
        }
    }
}