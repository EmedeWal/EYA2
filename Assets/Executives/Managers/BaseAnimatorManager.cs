using UnityEngine;
using System;

public abstract class BaseAnimatorManager : MonoBehaviour
{
    public Animator Animator { get; private set; }

    [HideInInspector] public float MovementSpeed;
    [HideInInspector] public float AttackSpeed;

    protected float _DeltaTime;

    private int _movementSpeedHash;
    private int _attackSpeedHash;
    private int _locomotionHash;
    private int _inActionHash;

    public event Action<BaseAnimatorManager> DeathAnimationFinished;

    public virtual void Init(float movementSpeed = 1, float attackSpeed = 1)
    {
        Animator = GetComponent<Animator>();

        MovementSpeed = movementSpeed;
        AttackSpeed = attackSpeed;

        _movementSpeedHash = Animator.StringToHash("MovementSpeed");
        _attackSpeedHash = Animator.StringToHash("AttackSpeed");
        _locomotionHash = Animator.StringToHash("Locomotion");
        _inActionHash = Animator.StringToHash("InAction");

        Animator.SetFloat(_movementSpeedHash, MovementSpeed);
        Animator.SetFloat(_attackSpeedHash, AttackSpeed);
    }

    public virtual void Tick(float deltaTime, float locomotion, float transitionTime = 0.4f)
    {
        _DeltaTime = deltaTime;

        Animator.SetFloat(_movementSpeedHash, MovementSpeed, 0.1f, _DeltaTime);
        Animator.SetFloat(_attackSpeedHash, AttackSpeed, 0.1f, _DeltaTime);
        Animator.SetFloat(_locomotionHash, locomotion, transitionTime, _DeltaTime);
    }

    public void ForceCrossFade(string animationName, bool allowRepeat, int layer = 1, float transitionDuration = 0.1f)
    {
        AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(layer);
        if (!currentState.IsName(animationName) || allowRepeat)
        {
            Animator.CrossFadeInFixedTime(animationName, transitionDuration, layer, _DeltaTime);
        }
    }

    public void SetFloat(int hash, float value, float transitionTime = 0.1f)
    {
        Animator.SetFloat(hash, value, transitionTime, _DeltaTime); 
    }

    public void SetBool(int hash, bool value)
    {
        Animator.SetBool(hash, value);
    }

    public float GetFloat(int hash)
    {
        return Animator.GetFloat(hash);
    }

    public bool GetBool(int hash)
    {
        return Animator.GetBool(hash);
    }

    public void CrossFade(string animationName, int layer = 1, float transitionDuration = 0.1f)
    {
        if (!Animator.IsInTransition(layer) && !Animator.GetBool(_inActionHash))
        {
            Animator.CrossFadeInFixedTime(animationName, transitionDuration, layer, _DeltaTime);
        }
    }

    public void CrossFadeAction(int animationHash, int layer = 1, float transitionDuration = 0.1f, bool canOverride = false)
    {
        if ((!Animator.IsInTransition(layer) && !Animator.GetBool(_inActionHash)) || canOverride)
        {
            SetBool(_inActionHash, true);
            Animator.CrossFadeInFixedTime(animationHash, transitionDuration, layer, _DeltaTime);
        }
    }


    public void OnDeathAnimationFinished()
    {
        DeathAnimationFinished?.Invoke(this);
    }
}