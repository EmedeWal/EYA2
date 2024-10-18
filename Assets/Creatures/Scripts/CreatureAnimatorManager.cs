using UnityEngine;

public class CreatureAnimatorManager : AnimatorManager
{
    public float Focus { get; private set; }

    private int _animatorLocomotion;

    public void InitCreature(Health health, float focus, float movementSpeed = 1, float attackSpeed = 1)
    {
        base.Init(movementSpeed, attackSpeed);

        health.ValueRemoved += CreatureAnimatorManager_ValueRemoved;

        Focus = focus;

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

    private void CreatureAnimatorManager_ValueRemoved(float amount)
    {
        if (!Animator.GetBool("InAction") && amount > Focus)
        {
            Animator.SetTrigger("Stagger");
        }
    }
}