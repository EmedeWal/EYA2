using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvasionPerk", menuName = "Perks/PassivePerks/EvasionPerk")]
public class EvasionPerk : PerkData
{
    [Header("VARIABLES")]
    [SerializeField] private float _maxEvasionChanceIncrease = 25f;
    [SerializeField] private float _completionTime = 10f;
    private float _currentEvasionChanceIncrease;
    private float _evasionChanceIncrement;
    private float _evasionTimer;

    [Header("SHIELD")]
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private int _shieldCount = 1;
    [SerializeField] private bool _damageReflection = false;
    [SerializeField] private bool _manaRestoration = false;
    private GameObject _currentShield;
    private int _currentShieldCount;
    private float _shieldTimer;

    [Header("EXPLOSION")]
    [SerializeField] private Explosion _shieldExplosionPrefab;
    [SerializeField] private float _radius = 4;
    private LayerMask _targetLayers;

    private Health _playerHealth;
    private Mana _playerMana;

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

        _evasionChanceIncrement = _maxEvasionChanceIncrease / _completionTime;
        _currentEvasionChanceIncrease = 0;
        _evasionTimer = 0;

        _currentShieldCount = _shieldCount;
        _shieldTimer = _completionTime;

        _targetLayers = LayerMask.NameToLayer("Damage Collider");

        _playerHealth = _PlayerObject.GetComponent<Health>();
        _playerMana = _PlayerObject.GetComponent<Mana>();
    }

    public override void Activate()
    {
        _playerHealth.ValueRemoved += EvasionPerk_ValueRemoved;
        ResetPerk();
    }

    public override void Deactivate()
    {
        _playerHealth.ValueRemoved -= EvasionPerk_ValueRemoved;
        ResetPerk();
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

        if (_shieldPrefab != null && _currentShield == null)
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
        ResetPerk();
    }

    private void EvasionPerk_HitShielded(GameObject attackerObject, float damageShielded)
    {
        _currentShieldCount--;

        if (_currentShieldCount <= 0)
        {
            EnableShield(false);
        }

        if (_shieldExplosionPrefab != null)
        {
            Explosion explosion = Instantiate(_shieldExplosionPrefab, _PlayerTransform);
            VFX explosionVFX = explosion.GetComponent<VFX>();
            VFXManager.Instance.AddVFX(explosionVFX, explosion.transform, true, 5);
            explosion.Init(_radius, _targetLayers);
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
        if (enabled)
        {
            _playerHealth.Shielded = true;
            _playerHealth.HitShielded += EvasionPerk_HitShielded;
            _currentShield = Instantiate(_shieldPrefab, _PlayerTransform);
        }
        else if (_currentShield != null)
        {
            _playerHealth.HitShielded -= EvasionPerk_HitShielded;
            _currentShieldCount = _shieldCount;
            _playerHealth.Shielded = false;
            Destroy(_currentShield);
        }
    }

    private void ResetPerk()
    {
        _PlayerStats.IncrementStat(Stat.EvasionChance, -_currentEvasionChanceIncrease);
        _currentEvasionChanceIncrease = 0;
        _evasionTimer = 0;

        EnableShield(false);
        _shieldTimer = _completionTime;
    }
}
