using UnityEngine;

public class PlayerAnimatorManager : AnimatorManager
{
    private int _animatorHorizontal;
    private int _animatorVertical;
    private int _animatorSpeed;

    public override void Init(float movementSpeed = 1, float attackSpeed = 1)
    {
        base.Init(movementSpeed, attackSpeed);

        _animatorHorizontal = Animator.StringToHash("Horizontal");
        _animatorVertical = Animator.StringToHash("Vertical");
        _animatorSpeed = Animator.StringToHash("Speed");
    }

    public void UpdateAnimatorValues(float delta, float horizontal, float vertical, bool grounded, bool locked, bool moving)
    {
        if (!grounded)
        {
            horizontal = 0;
            vertical = 0;
        }

        float speed = moving ? 1 : 0;

        Animator.SetFloat(_AnimatorMovementSpeed, MovementSpeed, 0.1f, delta);
        Animator.SetFloat(_AnimatorAttackSpeed, AttackSpeed, 0.1f, delta);
        Animator.SetFloat(_animatorHorizontal, horizontal, 0.1f, delta);
        Animator.SetFloat(_animatorVertical, vertical, 0.1f, delta);
        Animator.SetFloat(_animatorSpeed, speed, 0.1f, delta);
        Animator.SetBool("Grounded", grounded);
        Animator.SetBool("Locked", locked);
    }
}