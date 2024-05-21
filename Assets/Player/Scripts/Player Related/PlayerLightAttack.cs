using System;
using UnityEngine;

public class PlayerLightAttack : PlayerAttack
{
    public event Action<Collider> SuccesfulLightAttack;

    private void OnEnable()
    {
        InputManager.LightAttackInput_Performed += PlayerLightAttack_LightAttackInput_Performed;
        SuccesfulAttack += PlayerLightAttack_SuccesfulAttack;
    }

    private void OnDisable()
    {
        InputManager.LightAttackInput_Performed -= PlayerLightAttack_LightAttackInput_Performed;
        SuccesfulAttack -= PlayerLightAttack_SuccesfulAttack;
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

    private void PlayerLightAttack_SuccesfulAttack(Collider hit)
    {
        SuccesfulLightAttack?.Invoke(hit);
    }
}
