using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComboData : ScriptableObject
{
    protected VFXManager _VFXManager;
    protected Transform _Transform;

    [Header("COMBO VARIABLES")]
    [SerializeField] private List<AttackType> _comboSequence;
    [SerializeField] private List<AttackType> _finisherSequence;
    [SerializeField] private float _leeway = 0.5f;
    private int _currentComboIndex;
    private int _currentFinisherIndex;
    private float _timer;

    [Header("VISUALISATION")]
    [SerializeField] private VFX _completedVFX;

    private bool _paused;
    private bool _ready;
    private bool _tick;

    public event Action ComboFinished;

    public virtual void Init(VFXManager VFXManager, Transform transform)
    {
        _VFXManager = VFXManager;
        _Transform = transform;

        ResetCombo();
    }

    public void RegisterAttackStarted(AttackType attackType)
    {
        if (_paused)
        {
            ResetCombo();
        }
        else if (_ready)
        {
            if (_finisherSequence[_currentFinisherIndex] == attackType)
            {
                _tick = false;
                _currentFinisherIndex++;
                PerformComboFinisher(attackType);
            }
            else
            {
                ResetCombo();
            }
        }
    }

    public void RegisterAttackFinished(AttackType attackType)
    {
        if (_currentFinisherIndex >= _finisherSequence.Count)
        {
            OnComboFinisherPerformed();
        }
        else if (_currentComboIndex < _comboSequence.Count)
        {
            if (_comboSequence[_currentComboIndex] == attackType)
            {
                _currentComboIndex++;
                if (_currentComboIndex >= _comboSequence.Count)
                {
                    _tick = true;
                    _paused = true;
                    _timer = 0;
                }
            }
            else
            {
                ResetCombo();
            }
        }
    }

    public void Tick(float delta)
    {
        if (_tick)
        {
            if (_paused)
            {
                _timer += delta;
                if (_timer >= _leeway)
                {
                    _timer = 0;
                    _paused = false;
                    _ready = true;
                    _VFXManager.AddVFX(_completedVFX, true, 1f, _Transform.position, _Transform.rotation, _Transform);
                }
            }
            else if (_ready)
            {
                _timer += delta;
                if (_timer >= _leeway * 2)
                {
                    ResetCombo();
                }
            }
        }
    }

    public void ResetCombo()
    {
        _currentComboIndex = 0;
        _currentFinisherIndex = 0;
        _timer = 0f;
        _paused = false;
        _ready = false;
        _tick = false;
    }

    protected abstract void PerformComboFinisher(AttackType attackType);

    private void OnComboFinisherPerformed()
    {
        ComboFinished?.Invoke();
    }
}