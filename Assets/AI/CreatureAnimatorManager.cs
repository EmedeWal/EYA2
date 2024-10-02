using UnityEngine;

public class CreatureAnimatorManager : AnimatorManager
{
    private int _animatorMovement;

    public override void Init(float movementSpeed = 1, float attackSpeed = 1)
    {
        base.Init(movementSpeed, attackSpeed);

        _animatorMovement = Animator.StringToHash("Movement");
    }

    public void UpdateAnimatorValues(float delta, float movement)
    {
        _Animator.SetFloat(_animatorMovement, movement, 0.1f, delta);
        _Animator.SetFloat(_AnimatorMovementSpeed, MovementSpeed, 0.1f, delta);
        _Animator.SetFloat(_AnimatorAttackSpeed, AttackSpeed, 0.1f, delta);
    }
}
