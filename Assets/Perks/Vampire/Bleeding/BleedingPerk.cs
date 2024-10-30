using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bleeding Perk", menuName = "Scriptable Object/Perks/Passive Perk/Bleeding")]
public class BleedingPerk : PassivePerk
{
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
    [SerializeField] private float _damageReductionModifier = 0.05f;
    [SerializeField] private float _damageInflictedModifier = 0.05f;
    [SerializeField] private float _damageModifier = 1f;
    [SerializeField] private float _durationModifier = 1f;

    private List<BleedHandler> _bleedingEnemies;
    private BleedingStats _currentBleedingStats; 

    public override void Init(GameObject playerObject, List<PerkData> perks = null, Dictionary<Stat, float> statChanges = null)
    {
        base.Init(playerObject, perks, statChanges);

        _bleedingEnemies = new();
    }

    public override void Tick(float delta)
    {
        foreach (BleedHandler handler in _bleedingEnemies)
        {
            handler.Tick();
        }
    }

    public override void Activate()
    {
        CreatureManager.CreatureDeath += BleedingPerk_CreatureDeath;
        _AttackHandler.SuccessfulHit += BleedingPerk_SuccesfulHit;
    }

    public override void Deactivate()
    {
        CreatureManager.CreatureDeath -= BleedingPerk_CreatureDeath;
        _AttackHandler.SuccessfulHit -= BleedingPerk_SuccesfulHit;
    }

    private void BleedingPerk_SuccesfulHit(Collider hit, float damage, bool crit)
    {
        if (hit.TryGetComponent(out CreatureAI creature))
        {
            BleedHandler bleedHandler = creature.BleedHandler;

            UpdateBleedingStats();
            bleedHandler.ApplyBleed(_currentBleedingStats);

            if (!_bleedingEnemies.Contains(bleedHandler))
            {
                AddBleedingInstance(bleedHandler);
            }   
            else
            {
                _Health.Heal(bleedHandler.CurrentStacks * _healthGain);
            }
        }
    }

    private void BleedingPerk_CreatureDeath(CreatureAI creature)
    {
        BleedHandler bleedHandler = creature.BleedHandler;

        if (_bleedingEnemies.Contains(bleedHandler))
        {
            bleedHandler.ResetBleed();

            if (_bloodEruptionVFX != null)
            {
                Transform transform = creature.transform;
                int stacks = bleedHandler.CurrentStacks;

                VFX bloodEruptionVFX = _VFXManager.AddStaticVFX(_bloodEruptionVFX, transform.position, transform.rotation, 3f);

                UpdateBleedingStats();
                BloodEruption bloodEruption = bloodEruptionVFX.GetComponent<BloodEruption>();
                bloodEruption.InitBloodEruption(_bloodEruptionRadius, 1f, stacks, _TargetLayer, _currentBleedingStats);
            }
        }
    }

    private void BleedingPerk_BleedFinished(BleedHandler bleedHandler)
    {
        RemoveBleedingInstance(bleedHandler);
    }

    private void UpdateBleedingStats()
    {
        int enemyCount = _bleedingEnemies.Count;

        int currentMaxStacks = _maxStacks + _stacksBonus * enemyCount;
        float currentDamage = (_damage + _damageBonus * enemyCount) * _damageModifier;
        float currentDuration = (_duration + _durationBonus * enemyCount) * _durationModifier;

        _currentBleedingStats = new BleedingStats(currentMaxStacks, currentDamage, currentDuration, _damageReductionModifier, _damageInflictedModifier);
    }

    private void AddBleedingInstance(BleedHandler bleedHandler)
    {
        _bleedingEnemies.Add(bleedHandler);
        bleedHandler.BleedFinished += BleedingPerk_BleedFinished;
    }

    private void RemoveBleedingInstance(BleedHandler bleedHandler)
    {
        _bleedingEnemies.Remove(bleedHandler);
        bleedHandler.BleedFinished -= BleedingPerk_BleedFinished;
    }
}