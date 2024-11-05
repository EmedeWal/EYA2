using UnityEngine;
using System;

public abstract class BaseAnimatorManager : MonoBehaviour
{
    public Animator Animator { get; private set; }

    [HideInInspector] public float MovementSpeed;
    [HideInInspector] public float AttackSpeed;

    protected int _AnimatorMovementSpeed;
    protected int _AnimatorAttackSpeed;
    protected int _AnimatorLocomotion;
    protected float _Delta;
    
    private bool _crossFading = false;

    public event Action<BaseAnimatorManager> DeathAnimationFinished;

    public virtual void Init(float movementSpeed = 1, float attackSpeed = 1)
    {
        Animator = GetComponent<Animator>();

        MovementSpeed = movementSpeed;
        AttackSpeed = attackSpeed;

        _AnimatorMovementSpeed = Animator.StringToHash("MovementSpeed");
        _AnimatorAttackSpeed = Animator.StringToHash("AttackSpeed");
        _AnimatorLocomotion = Animator.StringToHash("Locomotion");

        Animator.SetFloat(_AnimatorMovementSpeed, MovementSpeed);
        Animator.SetFloat(_AnimatorAttackSpeed, AttackSpeed);
    }

    public virtual void Tick(float delta, float locomotion)
    {
        _Delta = delta;

        Animator.SetFloat(_AnimatorMovementSpeed, MovementSpeed, 0.1f, _Delta);
        Animator.SetFloat(_AnimatorAttackSpeed, AttackSpeed, 0.1f, _Delta);
        Animator.SetFloat(_AnimatorLocomotion, locomotion, 0.1f, _Delta);
    }

    public void ForceCrossFade(float delta, string animationName, float transitionDuration = 0.1f, int layer = 1)
    {
        CancelInvoke();
        _crossFading = true;
        Invoke(nameof(ResetCrossFading), transitionDuration + 0.25f);
        Animator.CrossFade(animationName, transitionDuration, layer, delta);
    }

    public void SetBool(string name, bool value)
    {
        Animator.SetBool(name, value);
    }

    public bool GetBool(string boolName)
    {
        return Animator.GetBool(boolName);
    }

    public bool CrossFade(float delta, string animationName, float transitionDuration = 0.1f, int layer = 1)
    {
        if (!_crossFading && !Animator.GetBool("InAction"))
        {
            _crossFading = true;
            Invoke(nameof(ResetCrossFading), transitionDuration + 0.1f);
            Animator.CrossFade(animationName, transitionDuration, layer, delta);
        }

        return _crossFading;
    }

    public void OnDeathAnimationFinished()
    {
        DeathAnimationFinished?.Invoke(this);
    }

    private void ResetCrossFading()
    {
        _crossFading = false;
    }
}