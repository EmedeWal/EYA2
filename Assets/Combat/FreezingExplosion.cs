using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FreezingExplosion : AreaOfEffect
{
    [Header("VFX")]
    [SerializeField] private VFX _freezeVFX;
    private VFXManager _vfxManager;

    [Header("VARIABLES")]
    [SerializeField] private float _slowPercentage;
    [SerializeField] private float _slowDuration;

    private Dictionary<CreatureAI, VFX> _freezeVFXDictionary;
    private Dictionary<Stat, float> _baseStats;

    public override void Init(float radius, LayerMask targetLayers, Collider colliderToIgnore = null)
    {
        CreatureManager.CreatureDeath += OnCreatureDeath;

        _vfxManager = VFXManager.Instance;

        _freezeVFXDictionary = new();
        _baseStats = new()
        {
            { Stat.MovementSpeedModifier, 0f },
            { Stat.AttackSpeedModifier, 0f },
        };

        base.Init(radius, targetLayers, colliderToIgnore);

        Invoke(nameof(Cleanup), _slowDuration);
    }

    public void Cleanup()
    {
        CreatureManager.CreatureDeath -= OnCreatureDeath;
        foreach (var entry in _freezeVFXDictionary)
        {
            _vfxManager.RemoveVFX(entry.Value);
        }
        _freezeVFXDictionary.Clear();
    }

    protected override void Effect(Collider hit)
    {
        if (hit.TryGetComponent(out CreatureAI creature))
        {
            ApplySlowEffect(creature);
        }
    }

    private void ApplySlowEffect(CreatureAI creature)
    {
        if (_freezeVFXDictionary.ContainsKey(creature)) return;

        VFX freezeVFX = _vfxManager.AddMovingVFX(_freezeVFX, creature.transform);
        _freezeVFXDictionary.Add(creature, freezeVFX);

        CreatureStatManager statManager = creature.StatManager;
        CreatureStatTracker statTracker = new(_baseStats, statManager);

        ApplyStatModifier(statManager, statTracker, Stat.MovementSpeedModifier);
        ApplyStatModifier(statManager, statTracker, Stat.AttackSpeedModifier);

        StartCoroutine(RestoreSpeedAfterDuration(creature, statManager, statTracker));
    }

    private void ApplyStatModifier(CreatureStatManager statManager, CreatureStatTracker statTracker, Stat stat)
    {
        float currentModifier = statManager.GetStat(stat);
        float slowAmount = currentModifier * (1f - (_slowPercentage / 100f));

        statTracker.IncrementStat(stat, slowAmount - currentModifier);
    }

    private IEnumerator RestoreSpeedAfterDuration(CreatureAI creature, CreatureStatManager statManager, CreatureStatTracker statTracker)
    {
        yield return new WaitForSeconds(_slowDuration);

        statTracker.ResetStatChanges();

        if (_freezeVFXDictionary.TryGetValue(creature, out VFX freezeVFX))
        {
            _vfxManager.RemoveVFX(freezeVFX);
            _freezeVFXDictionary.Remove(creature);
        }
    }

    private void OnCreatureDeath(CreatureAI creature)
    {
        if (_freezeVFXDictionary.TryGetValue(creature, out VFX freezeVFX))
        {
            _vfxManager.RemoveVFX(freezeVFX);
            _freezeVFXDictionary.Remove(creature);
        }
    }
}