public class PlayerLightAttack : PlayerAttack
{
    private void OnEnable()
    {
        _InputManager.LightAttackInput_Performed += PlayerLightAttack_LightAttackInput_Performed;
    }

    private void OnDisable()
    {
        _InputManager.LightAttackInput_Performed -= PlayerLightAttack_LightAttackInput_Performed;
    }

    private void PlayerLightAttack_LightAttackInput_Performed()
    {
        if (_CanAttack)
        {
            _Animator.SetFloat("Speed", 0);
            _Animator.SetTrigger("Light Attack");
            StartCharging();
        }
    }
}
