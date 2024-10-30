using UnityEngine;

public class CreatureHealth : Health
{
    [Header("STAGGER")]
    [SerializeField] private float _staggerRecovery;
    [SerializeField] private float _staggerThreshold;

    private CreatureAnimatorManager _animatorManager;
    private float _currentStaggerValue;

    public override void Init(float maxValue, float currentValue)
    {
        base.Init(maxValue, currentValue);

        _animatorManager = GetComponent<CreatureAnimatorManager>();

        _currentStaggerValue = 0;
    }

    public override void LateTick(float delta)
    {
        base.LateTick(delta);

        _currentStaggerValue -= _staggerRecovery * _Delta;
    }

    protected override void OnValueRemoved(float amount)
    {
        base.OnValueRemoved(amount);

        amount = Mathf.Abs(amount);

        _currentStaggerValue += amount;
        if (_currentStaggerValue >= _staggerThreshold)
        {
            _animatorManager.ForceCrossFade(_Delta, "Stagger");
            _currentStaggerValue = 0;
        }
    }
}