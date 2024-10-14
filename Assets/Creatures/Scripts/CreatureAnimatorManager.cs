using UnityEngine;

public class CreatureAnimatorManager : AnimatorManager
{
    private int _animatorLocomotion;

    public void InitCreature(Health health, float movementSpeed = 1, float attackSpeed = 1)
    {
        base.Init(movementSpeed, attackSpeed);

        health.ValueRemoved += CreatureAnimatorManager_ValueRemoved;

        _animatorLocomotion = Animator.StringToHash("Locomotion");
    }

    public void CleanupCreature(Health health)
    {
        health.ValueRemoved -= CreatureAnimatorManager_ValueRemoved;
    }

    public void Tick(float delta, float locomotion)
    {
        Animator.SetFloat(_AnimatorMovementSpeed, MovementSpeed, 0.1f, delta);
        Animator.SetFloat(_AnimatorAttackSpeed, AttackSpeed, 0.1f, delta);
        Animator.SetFloat(_animatorLocomotion, locomotion, 0.1f, delta);
    }

    private void CreatureAnimatorManager_ValueRemoved()
    {
        if (!Animator.GetBool("InAction"))
        {
            Animator.SetTrigger("Stagger");
        }
    }
}
