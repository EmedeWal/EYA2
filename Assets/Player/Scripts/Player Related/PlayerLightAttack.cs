using UnityEngine;

public class PlayerLightAttack : PlayerAttack
{
    private void OnEnable()
    {
        InputManager.LightAttackInput_Performed += PlayerLightAttack_LightAttackInput_Performed;
    }

    private void OnDisable()
    {
        InputManager.LightAttackInput_Performed -= PlayerLightAttack_LightAttackInput_Performed;
    }

    private void PlayerLightAttack_LightAttackInput_Performed()
    {
        if (CanAttack)
        {
            Animator.SetFloat("Speed", 0);
            Animator.SetTrigger("Light Attack");
            StartCharging();
        }
    }
}
