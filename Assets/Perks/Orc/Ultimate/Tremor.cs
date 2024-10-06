using System.Collections.Generic;
using UnityEngine;

public class Tremor : ConstantAreaDamage
{
    private Dictionary<AnimatorManager, float> _slowedEnemies = new();
    private List<VFX> _activeVFXs = new();

    [Header("VISUALISATION")]
    [SerializeField] private VFX _slowVFX;
    private float _maxSlowPercentage;
    private float _slowSpeed;

    private VFXManager _VFXManager;

    public void InitTremor(float radius, float damage, float slowSpeed, float maxSlow, LayerMask targetLayer, Collider colliderToIgnore = null)
    {
        _maxSlowPercentage = maxSlow;
        _slowSpeed = slowSpeed;

        _VFXManager = VFXManager.Instance;

        base.Init(radius, damage, targetLayer, colliderToIgnore);
    }

    protected override void Effect(Collider collider, float delta)
    {
        base.Effect(collider, delta);

        if (collider.TryGetComponent(out AnimatorManager animatorManager))
        {
            float currentSpeed = animatorManager.MovementSpeed;
            float maxSlowValue = 1f - (_maxSlowPercentage / 100f);
            float newSpeed = Mathf.Max(currentSpeed - _slowSpeed * delta, maxSlowValue);

            float appliedSlowAmount = currentSpeed - newSpeed;
            animatorManager.MovementSpeed = newSpeed;

            if (!_slowedEnemies.ContainsKey(animatorManager))
            {
                _slowedEnemies.Add(animatorManager, appliedSlowAmount);

                VFX vfxInstance = Instantiate(_slowVFX, animatorManager.transform.position, Quaternion.identity);
                _VFXManager.AddVFX(vfxInstance, animatorManager.transform); 
                _activeVFXs.Add(vfxInstance);
            }
            else
            {
                _slowedEnemies[animatorManager] += appliedSlowAmount;
            }
        }
    }

    public void Deactivate()
    {
        foreach (var enemy in _slowedEnemies)
        {
            AnimatorManager animatorManager = enemy.Key;
            float totalSlowApplied = enemy.Value;

            animatorManager.MovementSpeed += totalSlowApplied;
        }

        _slowedEnemies.Clear();

        foreach (VFX vfx in _activeVFXs)
        {
            _VFXManager.RemoveVFX(vfx, 1);
        }

        _activeVFXs.Clear();
    }
}
