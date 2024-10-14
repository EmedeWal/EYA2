using UnityEngine;

public abstract class AnimatorManager : MonoBehaviour
{
    public Animator Animator { get; private set; }

    [HideInInspector] public float MovementSpeed;
    [HideInInspector] public float AttackSpeed;

    protected int _AnimatorMovementSpeed;
    protected int _AnimatorAttackSpeed;

    public virtual void Init(float movementSpeed = 1, float attackSpeed = 1)
    {
        Animator = GetComponent<Animator>();

        MovementSpeed = movementSpeed;
        AttackSpeed = attackSpeed;

        _AnimatorMovementSpeed = Animator.StringToHash("Movement Speed");
        _AnimatorAttackSpeed = Animator.StringToHash("Attack Speed");
    }

    public void CrossFadeAnimation(float delta, string animationName, float transitionDuration = 0.1f, int layer = 1)
    {
        Animator.CrossFade(animationName, transitionDuration, layer, delta);
    }

    public float GetAnimationLength(int layer = 0)
    {
        return Animator.GetCurrentAnimatorStateInfo(layer).length;
    }

    public bool GetBool(string boolName)
    {
        return Animator.GetBool(boolName);
    }
}