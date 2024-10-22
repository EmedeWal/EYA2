using UnityEngine;

public class CreatureHealth : Health
{
    private CreatureAI _creatureAI;
    private CreatureData _creatureData;
    private CreatureAnimatorManager _animatorManager;

    private float _currentStaggerValue;

    public override void Init(float maxValue, float currentValue)
    {
        base.Init(maxValue, currentValue);

        _creatureAI = GetComponent<CreatureAI>();
        _animatorManager = GetComponent<CreatureAnimatorManager>();

        _creatureData = _creatureAI.CreatureData;

        _currentStaggerValue = 0;
    }

    public override void LateTick(float delta)
    {
        base.LateTick(delta);

        _currentStaggerValue -= _creatureData.StaggerRecovery * _Delta;
    }

    protected override void OnValueRemoved(float amount)
    {
        base.OnValueRemoved(amount);

        amount = Mathf.Abs(amount);

        _currentStaggerValue += amount;
        if (_currentStaggerValue >= _creatureData.StaggerThreshold)
        {
            _animatorManager.ForceCrossFade(_Delta, "Stagger");
            _currentStaggerValue = 0;
        }
    }
}