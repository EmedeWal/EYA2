using UnityEngine;

public class PlayerAnimatorManager : AnimatorManager
{
    private int _animatorHorizontal;
    private int _animatorVertical;

    public override void Init(float movementSpeed = 1, float attackSpeed = 1)
    {
        base.Init(movementSpeed, attackSpeed);

        _animatorHorizontal = Animator.StringToHash("Horizontal");
        _animatorVertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float delta, float horizontal, float vertical, bool grounded, bool lockedOn)
    {
        if (!grounded)
        {
            horizontal = 0;
            vertical = 0;
        }

        _Animator.SetFloat(_AnimatorMovementSpeed, MovementSpeed, 0.1f, delta);
        _Animator.SetFloat(_AnimatorAttackSpeed, AttackSpeed, 0.1f, delta);
        _Animator.SetFloat(_animatorHorizontal, horizontal, 0.1f, delta);
        _Animator.SetFloat(_animatorVertical, vertical, 0.1f, delta);
        _Animator.SetBool("LockedOn", lockedOn);
        _Animator.SetBool("Grounded", grounded);
    }
}
