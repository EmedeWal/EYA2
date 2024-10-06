using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvasionPerk", menuName = "Scriptable Object/Perks/Passive Perk/Evasion")]
public class EvasionPerk : PerkData
{
    [Header("VARIABLES")]
    [SerializeField] private float _maxEvasionChanceIncrease = 25f;
    [SerializeField] private float _completionTime = 10f;
    private float _currentEvasionChanceIncrease;
    private float _evasionChanceIncrement;
    private float _evasionTimer;

    [Header("SHIELD")]
    [SerializeField] private VFX _shieldPrefab;
    [SerializeField] private int _shieldCount = 1;
    [SerializeField] private bool _damageReflection = false;
    [SerializeField] private bool _manaRestoration = false;
    private VFX _currentShield;
    private int _currentShieldCount;
    private float _shieldTimer;
    private bool _shielded = false;

    [Header("EXPLOSION")]
    [SerializeField] private Explosion _shieldExplosionPrefab;
    [SerializeField] private float _radius = 4;
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
        _currentEvasionChanceIncrease = 0;
        _evasionTimer = 0;

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
        ResetPerkState();
        StartShieldGFX();
    }

    public override void Deactivate()
    {
        _playerHealth.ValueRemoved -= EvasionPerk_ValueRemoved;
        ResetPerkState();
    }

    public override void Tick(float delta)
    {
        if (_currentEvasionChanceIncrease < _maxEvasionChanceIncrease)
        {
            _evasionTimer += delta;
            if (_evasionTimer >= 1f)
            {
                _evasionTimer = 0f;
                _currentEvasionChanceIncrease += _evasionChanceIncrement;
                _PlayerStats.IncrementStat(Stat.EvasionChance, _evasionChanceIncrement);
            }
        }

        if (_shieldPrefab != null && !_shielded)
        {
            _shieldTimer -= delta;
            if (_shieldTimer <= 0)
            {
                EnableShield(true);
                _shieldTimer = _completionTime;
            }
        }
    }

    private void EvasionPerk_ValueRemoved()
    {
        ResetPerkState();
        StartShieldGFX();
    }

    private void EvasionPerk_HitShielded(GameObject attackerObject, float damageShielded)
    {
        _currentShieldCount--;

        if (_currentShieldCount <= 0)
        {
            EnableShield(false);
            StartShieldGFX();
        }

        if (_shieldExplosionPrefab != null)
        {
            Explosion explosion = Instantiate(_shieldExplosionPrefab, _PlayerTransform);
            VFX explosionVFX = explosion.GetComponent<VFX>();
            VFXManager.Instance.AddVFX(explosionVFX, explosion.transform, true, 5);
            explosion.Init(_radius, _targetLayer);
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
            _playerMana.GainMana(damageShielded);
        }
    }

    private void EnableShield(bool enabled)
    {
        if (_currentShield == null) return;

        _shielded = enabled;

        if (enabled)
        {
            _playerHealth.Shielded = true;
            _playerHealth.HitShielded += EvasionPerk_HitShielded;
            _currentShield.GetComponent<VFXPlayer>().PlayVFXInChildren();

            AudioSource source = _currentShield.GetComponent<AudioSource>();
            AudioSystem.Instance.PlayAudioClip(source, source.clip, source.volume);
        }
        else
        {
            _playerHealth.HitShielded -= EvasionPerk_HitShielded;
            _VFXManager.RemoveVFX(_currentShield);
            _currentShieldCount = _shieldCount;
            _playerHealth.Shielded = false;
        }
    }

    private void StartShieldGFX()
    {
        if (_shieldPrefab != null)
        {
            _currentShield = Instantiate(_shieldPrefab, _PlayerTransform);
            _VFXManager.AddVFX(_currentShield, _PlayerTransform);
        }
    }

    private void ResetPerkState()
    {
        _PlayerStats.IncrementStat(Stat.EvasionChance, -_currentEvasionChanceIncrease);
        _currentEvasionChanceIncrease = 0;
        _evasionTimer = 0;

        EnableShield(false);
        _shieldTimer = _completionTime;
    }
}
