using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkData", menuName = "Perks/EvasionPerk")]
public class EvasionPerk : PerkData
{
    [Header("PLAYER STATS REFERENCE")]
    [SerializeField] private PlayerStats _playerStats;

    [Header("VARIABLES")]
    [SerializeField] private float _maxEvasionChanceIncrease = 25f;
    [SerializeField] private float _completionTime = 10f;
    private float _currentEvasionChanceIncrease;
    private float _evasionChanceIncrement;

    [Header("SHIELD")]
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _shieldExplosionPrefab;
    [SerializeField] private int _shieldCount = 1;
    [SerializeField] private bool _damageReflection = false;
    [SerializeField] private bool _manaRestoration = false;
    private GameObject _currentShield;
    private int _currentShieldCount;

    private GameObject _playerObject;
    private Transform _playerTransform;
    private Health _playerHealth;
    private Mana _playerMana;

    private MonoHelperSystem _monoHelperSystem;

    public override void Init(List<PerkData> perks, GameObject playerObject)
    {
        for (int i = perks.Count - 1; i >= 0; i--)
        {
            PerkData perk = perks[i];
            if (perk.GetType() == GetType())
            {
                perk.Deactivate();perks.RemoveAt(i); 
            }
        }

        _evasionChanceIncrement = _maxEvasionChanceIncrease / _completionTime;
        _currentEvasionChanceIncrease = 0;

        _currentShieldCount = _shieldCount;

        _playerObject = playerObject;
        _playerTransform = _playerObject.transform;
        _playerHealth = _playerObject.GetComponent<Health>();
        _playerMana = _playerObject.GetComponent<Mana>();

        _monoHelperSystem = MonoHelperSystem.Instance;
    }

    public override void Activate()
    {
        _playerHealth.ValueRemoved += EvasionPerk_ValueRemoved;
        StartCoroutines();
    }

    public override void Deactivate()
    {
        _playerHealth.ValueRemoved -= EvasionPerk_ValueRemoved;
        StopCoroutines();
    }

    private void EvasionPerk_ValueRemoved()
    {
        StopCoroutines();
        StartCoroutines();
    }

    private void EvasionPerk_HitShielded(GameObject attackerObject, float damageShielded)
    { 
        _currentShieldCount--;

        if (_currentShieldCount <= 0)
        {
            EnableShield(false);
            _monoHelperSystem.StartCoroutine(ShieldCoroutine());
        }

        if (_shieldExplosionPrefab != null)
        {
            Instantiate(_shieldExplosionPrefab, _playerTransform);
        }

        if (_damageReflection)
        {
            if (attackerObject.TryGetComponent(out Health attackerHealth))
            {
                attackerHealth.TakeDamage(_playerObject, damageShielded);
            }
        }

        if (_manaRestoration)
        {
            _playerMana.GainMana(damageShielded);
        }
    }

    private void StartCoroutines()
    {
        _monoHelperSystem.StartCoroutine(IncreaseEvasionChanceCoroutine());

        if (_shieldPrefab != null)
        {
            _monoHelperSystem.StartCoroutine(ShieldCoroutine());
        }
    }

    private void StopCoroutines()
    {
        _playerStats.IncrementStat(Stat.EvasionChance, -_currentEvasionChanceIncrease);
        _monoHelperSystem.StopAllCoroutines();
        _currentEvasionChanceIncrease = 0;
        EnableShield(false);
    }

    private void EnableShield(bool enabled)
    {
        if (enabled)
        {
            _playerHealth.Shielded = true;
            _playerHealth.HitShielded += EvasionPerk_HitShielded;
            _currentShield = Instantiate(_shieldPrefab, _playerTransform);
        }
        else if (_currentShield != null)
        {
            _playerHealth.HitShielded -= EvasionPerk_HitShielded;
            _currentShieldCount = _shieldCount;
            _playerHealth.Shielded = false;
            Destroy(_currentShield);
        }
    }

    private IEnumerator IncreaseEvasionChanceCoroutine()
    {
        while (_currentEvasionChanceIncrease < _maxEvasionChanceIncrease)
        {
            yield return new WaitForSeconds(1);

            _currentEvasionChanceIncrease += _evasionChanceIncrement;
            _playerStats.IncrementStat(Stat.EvasionChance, _evasionChanceIncrement);
        }
    }

    private IEnumerator ShieldCoroutine()
    {
        yield return new WaitForSeconds(_completionTime);

        EnableShield(true);
    }
}
