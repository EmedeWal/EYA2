using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Orc Ultimate", menuName = "Scriptable Object/Perks/Ultimate Perk/Orc")]
public class OrcUltimate : PerkData
{
    [Header("SHOCKWAVES")]
    [SerializeField] private VFX _shockwaveVFX;
    [SerializeField] private float _shockwaveScaling = 1f;
    [SerializeField] private bool _repeatShockwaves = false;

    [Header("TREMORS")]
    [SerializeField] private VFX _tremorVFX;
    [SerializeField] private float _tremorRadius = 4f;
    [SerializeField] private float _tremorDamage = 10f;
    [SerializeField] private float _tremorSlowSpeed = 15f;
    [SerializeField] private float _tremorSlowPercentage = 70f;
    private Tremor _currentTremor;
    private VFX _currentTremorVFX;

    [Header("BATTLE ZONE")]
    [SerializeField] private VFX _battleZoneVFX;
    [SerializeField] private float _criticalChance = 25f;
    [SerializeField] private float _criticalMultiplier = 0.5f;
    [SerializeField] private float _damageReduction = 5f;
    [SerializeField] private float _healthRegen = 1;
    private BattleZone _currentBattleZone;
    private VFX _currentBattleZoneVFX;

    [Header("SLASH EFFECT")]
    [SerializeField] private VFX _slashVFX;
    [SerializeField] private float _slashRadius = 1f;
    [SerializeField] private float _slashDamagePercentage = 50f;

    public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
    {
        statChanges = new()
        {
            { Stat.CriticalChance, 0 },
            { Stat.CriticalMultiplier, 0 },
            { Stat.HealthRegen, 0 },
            { Stat.DamageReduction, 0 },
        };

        base.Init(playerObject, perks, statChanges);
    }

    public override void Activate()
    {
        if (_shockwaveVFX != null)
        {
            CastShockwaves(_PlayerTransform);
        }

        if (_tremorVFX != null)
        {
            CastTremor(true);
        }

        if (_battleZoneVFX != null)
        {
            CastBattleZone(true);
        }
    }

    public override void Tick(float delta)
    {
        if (_currentTremor != null)
        {
            _currentTremor.Tick(delta);
        }
    }

    public override void Deactivate()
    {
        if (_shockwaveVFX != null && _repeatShockwaves)
        {
            CastShockwaves(_currentTremor.transform);
        }

        if (_currentTremorVFX != null)
        {
            CastTremor(false);
        }

        if (_currentBattleZoneVFX != null)
        {
            CastBattleZone(false);
        }

        _StatTracker.ResetStatChanges();
    }

    private void CastShockwaves(Transform transform)
    {
        VFX shockwaveVFX = _VFXManager.AddStaticVFX(_shockwaveVFX, transform.position, transform.rotation, 3f);
        shockwaveVFX.GetComponent<Shockwave>().Init(_TargetLayer, _shockwaveScaling);

        AudioSource source = shockwaveVFX.GetComponent<AudioSource>();
        _AudioSystem.PlayAudio(source, source.clip, source.volume);
    }

    private void CastTremor(bool enable)
    {
        if (enable)
        {
            _currentTremorVFX = _VFXManager.AddStaticVFX(_tremorVFX, _PlayerTransform.position, _PlayerTransform.rotation);
            _currentTremor = _currentTremorVFX.GetComponent<Tremor>();
            _currentTremor.InitTremor(_tremorRadius, _tremorDamage, _tremorSlowSpeed, _tremorSlowPercentage, _TargetLayer);
        }
        else
        {
            _VFXManager.RemoveVFX(_currentTremorVFX, 1f);
            _currentTremor.Deactivate();
            _currentTremorVFX = null;
            _currentTremor = null;
        }
    }

    private void CastBattleZone(bool enable)
    {
        if (enable)
        {
            _currentBattleZoneVFX = _VFXManager.AddStaticVFX(_battleZoneVFX, _PlayerTransform.position, _PlayerTransform.rotation);
            _currentBattleZone = _currentBattleZoneVFX.GetComponent<BattleZone>();
            _currentBattleZone.Init(_StatTracker, _criticalChance, _criticalMultiplier, _damageReduction, _healthRegen);

            if (_slashVFX != null)
            {
                _AttackHandler.SuccessfulAttack += OrcUltimate_SuccessfulAttack;
            }
        }
        else
        {
            _VFXManager.RemoveVFX(_currentBattleZoneVFX);
            _currentBattleZoneVFX = null;
            _currentBattleZone = null;

            if (_slashVFX != null)
            {
                _AttackHandler.SuccessfulAttack -= OrcUltimate_SuccessfulAttack;
            }
        }
    }

    private void OrcUltimate_SuccessfulAttack(Collider hit, int colliders, float damage, bool crit)
    {
        if (_currentBattleZone.PlayerInside && crit)
        {
            VFX slashVFX = _VFXManager.AddStaticVFX(_slashVFX, _PlayerTransform.position, _PlayerTransform.rotation, 1f);

            Explosion explosion = slashVFX.GetComponent<Explosion>();
            float finalDamage = damage / 100 * _slashDamagePercentage;
            explosion.InitExplosion(_slashRadius, finalDamage, _TargetLayer, null, 0.05f);
        }
    }
}