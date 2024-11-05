using System.Collections.Generic;
using UnityEngine;

public class Tremor : ConstantAreaDamage
{
    private Dictionary<CreatureAI, VFX> _activeVFXDictionary;
    private Dictionary<CreatureAI, CreatureStatTracker> _slowedCreatures;

    [Header("VISUALISATION")]
    [SerializeField] private VFX _slowVFX;
    private float _minimumAnimatorSpeed;
    private float _animatorSlowSpeed;

    private VFXManager _VFXManager;

    public void InitTremor(float radius, float damage, float slowSpeed, float maxSlow, LayerMask targetLayer)
    {
        CreatureManager.CreatureDeath += Tremor_CreatureDeath;

        _activeVFXDictionary = new();
        _slowedCreatures = new();

        _minimumAnimatorSpeed = 1f - (maxSlow / 100);
        _animatorSlowSpeed = 1f - (slowSpeed / 100);

        _VFXManager = VFXManager.Instance;

        base.Init(radius, damage, targetLayer);
    }

    protected override void Effect(Collider collider, float delta)
    {
        base.Effect(collider, delta);

        if (collider.TryGetComponent(out CreatureAI creature) && _minimumAnimatorSpeed < 1)
        {
            CreatureStatManager statManager = creature.StatManager;
            BaseAnimatorManager animatorManager = creature.AnimatorManager;

            float currentSpeed = statManager.GetStat(Stat.MovementSpeedModifier);
            float newSpeed = Mathf.Max(currentSpeed - _animatorSlowSpeed * delta, _minimumAnimatorSpeed);
            float appliedSlowAmount = currentSpeed - newSpeed;

            if (!_slowedCreatures.ContainsKey(creature))
            {
                var baseStatChanges = new Dictionary<Stat, float> { { Stat.MovementSpeedModifier, 0f } };
                CreatureStatTracker statTracker = new(baseStatChanges, statManager);
                _slowedCreatures.Add(creature, statTracker);

                statTracker.IncrementStat(Stat.MovementSpeedModifier, newSpeed - currentSpeed);

                VFX vfxInstance = _VFXManager.AddMovingVFX(_slowVFX, animatorManager.transform);
                _activeVFXDictionary.Add(creature, vfxInstance);
            }
            else
            {
                var statTracker = _slowedCreatures[creature];
                statTracker.IncrementStat(Stat.MovementSpeedModifier, newSpeed - currentSpeed);
            }
        }
    }

    public void Deactivate()
    {
        CreatureManager.CreatureDeath -= Tremor_CreatureDeath;

        foreach (var creature in _activeVFXDictionary)
        {
            _VFXManager.RemoveVFX(creature.Value);
        }
        _activeVFXDictionary.Clear();

        foreach (var creature in _slowedCreatures)
        {
            creature.Value.ResetStatChanges();
        }
        _slowedCreatures.Clear();
    }

    private void Tremor_CreatureDeath(CreatureAI creature)
    {
        if (_activeVFXDictionary.ContainsKey(creature))
        {
            _VFXManager.RemoveVFX(_activeVFXDictionary[creature]);
            _activeVFXDictionary.Remove(creature);
        }

        if (_slowedCreatures.ContainsKey(creature))
        {
            _slowedCreatures[creature].ResetStatChanges();
            _slowedCreatures.Remove(creature);
        }
    }
}