using System;
using UnityEngine;

public class PlayerHeavyAttack : PlayerAttack
{
    public event Action<Collider> SuccesfulHeavyAttack;

    private void OnEnable()
    {
        InputManager.HeavyAttackInput_Performed += PlayerHeavyAttack_HeavyAttackInput_Performed;
        SuccesfulAttack += PlayerHeavyAttack_SuccesfulAttack;
    }

    private void OnDisable()
    {
        InputManager.HeavyAttackInput_Performed -= PlayerHeavyAttack_HeavyAttackInput_Performed;
        SuccesfulAttack += PlayerHeavyAttack_SuccesfulAttack;
    }

    private void PlayerHeavyAttack_HeavyAttackInput_Performed()
    {
        if (CanAttack)
        {
            Animator.SetFloat("Speed", 0);
            Animator.SetTrigger("Heavy Attack");
            StartCharging();
        }
    }

    private void PlayerHeavyAttack_SuccesfulAttack(Collider hit)
    {
        SuccesfulHeavyAttack?.Invoke(hit);
    }
}