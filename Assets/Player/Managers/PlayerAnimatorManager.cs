using UnityEngine;

public class PlayerAnimatorManager : BaseAnimatorManager
{
    private int _animatorHorizontal;
    private int _animatorVertical;

    public override void Init(float movementSpeed = 1, float attackSpeed = 1)
    {
        base.Init(movementSpeed, attackSpeed);

        _animatorHorizontal = Animator.StringToHash("Horizontal");
        _animatorVertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float delta, float horizontal, float vertical, bool grounded, bool locked, bool moving)
    {
        if (!grounded)
        {
            horizontal = 0;
            vertical = 0;
        }

        float locomotion = moving ? 1 : 0;

        base.Tick(delta, locomotion);

        Animator.SetFloat(_animatorHorizontal, horizontal, 0.1f, _Delta);
        Animator.SetFloat(_animatorVertical, vertical, 0.1f, _Delta);
        Animator.SetBool("Grounded", grounded);
        Animator.SetBool("Locked", locked);
    }
}