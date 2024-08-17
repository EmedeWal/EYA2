using UnityEngine;

public class PlayerHeavyAttack : PlayerAttack
{
    private void OnEnable()
    {
        _InputManager.HeavyAttackInput_Performed += PlayerHeavyAttack_HeavyAttackInput_Performed;
    }

    private void OnDisable()
    {
        _InputManager.HeavyAttackInput_Performed -= PlayerHeavyAttack_HeavyAttackInput_Performed;
    }

    private void PlayerHeavyAttack_HeavyAttackInput_Performed()
    {
        if (_CanAttack)
        {
            _Animator.SetFloat("Speed", 0);
            _Animator.SetTrigger("Heavy Attack");
            StartCharging();
        }
    }
}