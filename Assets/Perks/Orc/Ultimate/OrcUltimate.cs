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

    private AttackHandler _attackHandler;
    private LayerMask _targetLayer;

    private AudioSystem _audioSystem;
    private VFXManager _VFXManager;

    public override void Init(GameObject playerObject, List<PerkData> perks = null)
    {
        base.Init(playerObject, perks);

        _attackHandler = _PlayerObject.GetComponent<AttackHandler>();
        _targetLayer = LayerMask.GetMask("DamageCollider");

        _audioSystem = AudioSystem.Instance;
        _VFXManager = VFXManager.Instance;
    }

    public override void Activate()
    {
        if (_shockwaveVFX != null)
        {
            CastShockwaves();
        }

        if (_tremorVFX != null)
        {
            CastTremor();
        }

        if (_battleZoneVFX != null)
        {
            CastBattleZone();
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
            CastShockwaves();
        }

        if (_currentTremorVFX != null)
        {
            _VFXManager.RemoveVFX(_currentTremorVFX, 1f);
            _currentTremor.Deactivate();
            _currentTremorVFX = null;
            _currentTremor = null;
        }

        if (_currentBattleZoneVFX != null)
        {
            _VFXManager.RemoveVFX(_currentBattleZoneVFX, 1f);
            _currentBattleZone.Cleanup();
            _currentBattleZoneVFX = null;
            _currentBattleZone = null;

            if (_slashVFX != null)
            {
                _attackHandler.SuccessfulAttack -= OrcUltimate_SuccessfulAttack;
            }
        }
    }

    private void CastShockwaves()
    {
        VFX shockwaveVFX = _VFXManager.AddVFX(_shockwaveVFX, true, 3f, _PlayerTransform.position, _PlayerTransform.rotation);

        AudioSource source = shockwaveVFX.GetComponent<AudioSource>();
        _audioSystem.PlayAudioClip(source, source.clip, source.volume);

        shockwaveVFX.GetComponent<Shockwave>().Init(_targetLayer, _shockwaveScaling);
    }

    private void CastTremor()
    {
        _currentTremorVFX = Instantiate(_tremorVFX, _PlayerTransform);
        _VFXManager.AddVFX(_currentTremorVFX, _currentTremorVFX.transform);

        _currentTremor = _currentTremorVFX.GetComponent<Tremor>();
        _currentTremor.InitTremor(_tremorRadius, _tremorDamage, _tremorSlowSpeed, _tremorSlowPercentage, _targetLayer);
    }

    private void CastBattleZone()
    {
        _currentBattleZoneVFX = Instantiate(_battleZoneVFX, _PlayerTransform);
        _VFXManager.AddVFX(_currentBattleZoneVFX, _currentBattleZoneVFX.transform);

        _currentBattleZone = _currentBattleZoneVFX.GetComponent<BattleZone>();
        _currentBattleZone.Init(_PlayerStats, _criticalChance, _criticalMultiplier, _damageReduction, _healthRegen);

        if (_slashVFX != null)
        {
            _attackHandler.SuccessfulAttack += OrcUltimate_SuccessfulAttack;
        }
    }

    private void OrcUltimate_SuccessfulAttack(Collider hit, int colliders, float damage, bool crit)
    {
        if (_currentBattleZone.PlayerInside && crit)
        {
            VFX slashVFX = _VFXManager.AddVFX(_slashVFX, true, 1f, _PlayerTransform.position, _PlayerTransform.rotation);

            Explosion explosion = slashVFX.GetComponent<Explosion>();
            float finalDamage = damage / 100 * _slashDamagePercentage;
            explosion.InitExplosion(_slashRadius, finalDamage, _targetLayer, 0.05f);
        }
    }
}