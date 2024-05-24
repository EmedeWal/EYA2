using UnityEngine;

public class PlayerHeavyAttack : PlayerAttack
{
    private void OnEnable()
    {
        InputManager.HeavyAttackInput_Performed += PlayerHeavyAttack_HeavyAttackInput_Performed;
    }

    private void OnDisable()
    {
        InputManager.HeavyAttackInput_Performed -= PlayerHeavyAttack_HeavyAttackInput_Performed;
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
}