using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HorrorGrab : ConstantAreaDamage
{
    [Header("SLOW INTENSITY")]
    [SerializeField] private float _slowIntensity = 0.8f;

    private PlayerStatTracker _playerStatTracker;
    private VFXManager _VFXManager;
    private Health _creatureHealth;
    private VFX _currentVFX;

    public void InitHorrorGrab(float radius, float damage, float duration, LayerMask targetLayer, VFX currentVFX, Health creatureHealth, PlayerStats playerStats)
    {
        base.Init(radius, damage, targetLayer);

        Dictionary<Stat, float> statChanges = new()
        {
            { Stat.MovementSpeedModifier, 0 }
        };

        _playerStatTracker = new PlayerStatTracker(statChanges, playerStats);

        _VFXManager = VFXManager.Instance;
        _creatureHealth = creatureHealth;
        _currentVFX = currentVFX;

        StartCoroutine(HorrorGrabCoroutine(duration));
    }

    public void CleanupHorrorGrab()
    {
        _playerStatTracker.ResetStatChanges();
        _VFXManager.RemoveVFX(_currentVFX, 1f);
    }

    protected override void Effect(Collider collider, float delta)
    {
        if (collider.TryGetComponent(out Health health))
        {
            _creatureHealth.Heal(health.TakeDamage(_GameObject, _Damage * delta));
            
            if (collider.gameObject.CompareTag("Player") && _playerStatTracker.GetStatChange(Stat.MovementSpeedModifier) == 0)
            {
                _playerStatTracker.IncrementStat(Stat.MovementSpeedModifier, -_slowIntensity);
            }
        }
    }

    private IEnumerator HorrorGrabCoroutine(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            yield return null;

            float delta = Time.deltaTime;
            timer += delta;
            Tick(delta);
        }

        CleanupHorrorGrab();
    }
}