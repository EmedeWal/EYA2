using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bleeding Perk", menuName = "Scriptable Object/Perks/Passive Perk/Bleeding")]
public class BleedingPerk : PerkData
{
    private PlayerAttackHandler _playerAttackHandler;
    private Health _playerHealth;

    [Header("BASE BLEEDING STATS")]
    [SerializeField] private int _maxStacks = 5;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _duration = 5f;

    [Header("BLOOD TO POWER STATS")]
    [SerializeField] private int _stacksBonus = 0;
    [SerializeField] private float _damageBonus = 0f;
    [SerializeField] private float _durationBonus = 0f;

    [Header("BLOOD ERUPTION")]
    [SerializeField] private VFX _bloodEruptionVFX;
    [SerializeField] private float _bloodEruptionRadius;

    [Header("HEMORRHAGE STATS")]
    [SerializeField] private float _healthGain = 0f;
    [SerializeField] private float _damageModifier = 1f;
    [SerializeField] private float _durationModifier = 1f;

    private List<BleedHandler> _bleedingEnemies;
    private BleedingStats _currentBleedingStats; 

    private LayerMask _targetLayer;
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
        _playerHealth = _PlayerObject.GetComponent<Health>();

        _bleedingEnemies = new List<BleedHandler>();

        _targetLayer = LayerMask.GetMask("DamageCollider");

        _VFXManager = VFXManager.Instance;
    }

    public override void Activate()
    {
        _playerAttackHandler.SuccessfulHit += BleedingPerk_SuccesfulHit;
    }

    public override void Deactivate()
    {
        _playerAttackHandler.SuccessfulHit -= BleedingPerk_SuccesfulHit;
    }

    public override void Tick(float delta)
    {
        // Logic for any effects that need to be updated over time
    }

    private void BleedingPerk_SuccesfulHit(Collider hit, float damage, bool crit)
    {
        if (hit.TryGetComponent(out BleedHandler bleedHandler))
        {
            UpdateBleedingStats();

            bleedHandler.ApplyBleed(_currentBleedingStats);

            if (!_bleedingEnemies.Contains(bleedHandler))
            {
                AddBleedingInstance(bleedHandler, bleedHandler.GetComponent<Health>());
            }
            else
            {
                _playerHealth.Heal(bleedHandler.CurrentStacks * _healthGain);
            }
        }
    }

    private void BleedingPerk_BleedFinished(BleedHandler bleedHandler)
    {
        RemoveBleedingInstance(bleedHandler, bleedHandler.GetComponent<Health>());
    }

    private void BleedingPerk_ValueExhausted(GameObject healthObject)
    {
        RemoveBleedingInstance(healthObject.GetComponent<BleedHandler>(), healthObject.GetComponent<Health>());

        if (_bloodEruptionVFX != null)
        {
            Transform transform = healthObject.transform;
            int stacks = healthObject.GetComponent<BleedHandler>().CurrentStacks;

            VFX bloodEruptionVFX = _VFXManager.AddStaticVFX(_bloodEruptionVFX, transform.position, transform.rotation, 3f);

            UpdateBleedingStats();
            BloodEruption bloodEruption = bloodEruptionVFX.GetComponent<BloodEruption>();
            bloodEruption.InitBloodEruption(stacks, _currentBleedingStats, _bloodEruptionRadius, 1f, _targetLayer);
        }
    }

    private void UpdateBleedingStats()
    {
        int enemyCount = _bleedingEnemies.Count;

        int currentMaxStacks = _maxStacks + _stacksBonus * enemyCount;
        float currentDamage = (_damage + _damageBonus * enemyCount) * _damageModifier;
        float currentDuration = (_duration + _durationBonus * enemyCount) * _durationModifier;

        _currentBleedingStats = new BleedingStats(currentDamage, currentDuration, currentMaxStacks);
    }

    private void AddBleedingInstance(BleedHandler bleedHandler, Health health)
    {
        _bleedingEnemies.Add(bleedHandler);
        health.ValueExhausted += BleedingPerk_ValueExhausted;
        bleedHandler.BleedFinished += BleedingPerk_BleedFinished;
    }

    private void RemoveBleedingInstance(BleedHandler bleedHandler, Health health)
    {
        _bleedingEnemies.Remove(bleedHandler);
        health.ValueExhausted -= BleedingPerk_ValueExhausted;
        bleedHandler.BleedFinished -= BleedingPerk_BleedFinished;
    }
}
