using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Offense Perk", menuName = "Scriptable Object/Perks/Passive Perk/Offense")]
public class OffensePerk : PerkData
{
    private PlayerAttackHandler _playerAttackHandler;

    [Header("QUAKE DAMAGE")]
    [SerializeField] private VFX _quakeVFX;
    [SerializeField] private float _quakeRadius = 1f;
    [SerializeField] private float _quakeDamagePercentage = 30f;
    [SerializeField] private bool _ignoreInitialTarget = true;
    private Explosion _currentExplosion;
    private LayerMask _targetLayer;

    [Header("QUAKE BUFF")]
    [SerializeField] private VFX _quakeBuffVFX;
    [SerializeField] private float _quakeBuffDuration = 3f;
    [SerializeField] private int _quakeBuffTargetRequirement = 3;
    [SerializeField] private float _quakeBuffDamageIncrease = 0.3f;
    private VFX _currentQuakeBuffVFX;
    private float _currentDamageIncrease;
    private float _quakeBuffTimer;

    [Header("GRIT")]
    [SerializeField] private VFX _gritVFX;
    [SerializeField] private float _maximumInactivity = 3f;
    [SerializeField] private float _maxAttackDamageBonus = 0.3f;
    [SerializeField] private float _maxCriticalChanceBonus = 30f;
    [SerializeField] private float _maxCriticalMultiplierBonus = 0.5f;
    private VFXEmission _currentGritVFXEmission;
    private VFX _currentGritVFX;
    private float _previousAttackDamageBonus;
    private float _previousCriticalChanceBonus;
    private float _previousCriticalMultiplierBonus;
    private float _inactiveTimer;
    private float _gritValue;
    
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

        _playerAttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();

        _targetLayer = LayerMask.GetMask("DamageCollider");

        _audioSystem = AudioSystem.Instance;
        _VFXManager = VFXManager.Instance;

        ResetQuakeBuff();
        ResetGrit();
    }

    public override void Activate()
    {
        _playerAttackHandler.SuccessfulAttack += OffensePerk_SuccesfulAttack;
    }

    public override void Deactivate()
    {
        _playerAttackHandler.SuccessfulAttack -= OffensePerk_SuccesfulAttack;

        ResetQuakeBuff();
        ResetGrit();
    }

    public override void Tick(float delta)
    {
        if (_currentQuakeBuffVFX != null)
        {
            _quakeBuffTimer += delta;

            if (_quakeBuffTimer > _quakeBuffDuration)
            {
                ResetQuakeBuff();
            }
        }

        if (_gritVFX != null)
        {
            _inactiveTimer += delta;

            if (_inactiveTimer > _maximumInactivity)
            {
                ResetGrit();
            }
        }
    }

    private void OffensePerk_SuccesfulAttack(Collider hit, int colliders, float damage, bool crit)
    {
        if (_quakeVFX != null)
        {
            Transform hitTransform;

            if (hit.TryGetComponent(out LockTarget lockTarget))
            {
                hitTransform = lockTarget.Center;
            }
            else
            {
                hitTransform = hit.transform;
            }

            VFX quakeVFX = Instantiate(_quakeVFX, hitTransform.position, hitTransform.rotation);
            _VFXManager.AddVFX(quakeVFX, hitTransform, true, 1f);

            AudioSource source = quakeVFX.GetComponent<AudioSource>();
            _audioSystem.PlayAudioClip(source, source.clip, source.volume);

            if (!_ignoreInitialTarget) hit = null;

            _currentExplosion = quakeVFX.GetComponent<Explosion>();

            if (_quakeBuffVFX != null)
            {
                _currentExplosion.HitsDetected += OffensePerk_HitsDetected;
            }

            float finalDamage = damage / 100 * _quakeDamagePercentage;
            _currentExplosion.InitExplosion(_quakeRadius, finalDamage, _targetLayer);
        }

        if (_gritVFX != null)
        {
            if (colliders == 1)
            {
                _gritValue = Mathf.Clamp(_gritValue + damage, 0f, 100f);
                _inactiveTimer = 0;
                HandleGritBonuses();
            }
            else
            {
                ResetGrit();
            }
        }
    }

    private void OffensePerk_HitsDetected(int hits)
    {
        if (hits >= _quakeBuffTargetRequirement)
        {
            _quakeBuffTimer = 0;

            if (_currentQuakeBuffVFX == null)
            {
                _currentQuakeBuffVFX = Instantiate(_quakeBuffVFX, _PlayerTransform);
                _VFXManager.AddVFX(_currentQuakeBuffVFX, _PlayerTransform);
            }
            else
            {
                _PlayerStats.IncrementStat(Stat.AttackDamageModifier, -_currentDamageIncrease);
            }

            _currentDamageIncrease = _quakeBuffDamageIncrease;
            _PlayerStats.IncrementStat(Stat.AttackDamageModifier, _currentDamageIncrease);
        }

        _currentExplosion.HitsDetected -= OffensePerk_HitsDetected;
    }

    private void HandleGritBonuses()
    {
        float gritPercent = _gritValue / 100f;

        float newAttackDamageBonus = _maxAttackDamageBonus * gritPercent;
        float newCriticalChanceBonus = _maxCriticalChanceBonus * gritPercent;
        float newCriticalMultiplierBonus = _maxCriticalMultiplierBonus * gritPercent;

        float attackDamageBonusChange = newAttackDamageBonus - _previousAttackDamageBonus;
        float criticalChanceBonusChange = newCriticalChanceBonus - _previousCriticalChanceBonus;
        float criticalMultiplierChange = newCriticalMultiplierBonus - _previousCriticalMultiplierBonus;

        _PlayerStats.IncrementStat(Stat.AttackDamageModifier, attackDamageBonusChange);
        _PlayerStats.IncrementStat(Stat.CriticalChance, criticalChanceBonusChange);
        _PlayerStats.IncrementStat(Stat.CriticalMultiplier, criticalMultiplierChange);

        _previousAttackDamageBonus = newAttackDamageBonus;
        _previousCriticalChanceBonus = newCriticalChanceBonus;
        _previousCriticalMultiplierBonus = newCriticalMultiplierBonus;

        if (_currentGritVFX != null)
        {
            _currentGritVFXEmission.Tick(gritPercent * 25);
        }
        else
        {
            _currentGritVFX = Instantiate(_gritVFX, _PlayerTransform);
            _VFXManager.AddVFX(_currentGritVFX, _PlayerTransform);

            _currentGritVFXEmission = _currentGritVFX.GetComponent<VFXEmission>();
            _currentGritVFXEmission.Init(gritPercent * 25);
        }
    }

    private void ResetQuakeBuff()
    {
        _quakeBuffTimer = 0;

        _PlayerStats.IncrementStat(Stat.AttackDamageModifier, -_currentDamageIncrease);
        _currentDamageIncrease = 0;

        if (_currentQuakeBuffVFX != null)
        {
            _VFXManager.RemoveVFX(_currentQuakeBuffVFX);
        }
    }   

    private void ResetGrit()
    {
        _gritValue = 0f;
        _inactiveTimer = 0f;

        _PlayerStats.IncrementStat(Stat.AttackDamageModifier, -_previousAttackDamageBonus);
        _PlayerStats.IncrementStat(Stat.CriticalChance, -_previousCriticalChanceBonus);
        _PlayerStats.IncrementStat(Stat.CriticalMultiplier, -_previousCriticalMultiplierBonus);

        _previousAttackDamageBonus = 0f;
        _previousCriticalChanceBonus = 0f;
        _previousCriticalMultiplierBonus = 0f;

        if (_currentGritVFX != null)
        {
            _VFXManager.RemoveVFX(_currentGritVFX, 1);
            _currentGritVFXEmission = null;
            _currentGritVFX = null;
        }
    }
}
