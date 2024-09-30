using UnityEngine;

public class Mana : Resource
{
    private PlayerAttackHandler _playerAttackHandler;

    public override void Init(float maxValue, float currentValue)
    {
        base.Init(maxValue, currentValue);

        _playerAttackHandler = GetComponent<PlayerAttackHandler>();
        _playerAttackHandler.SuccessfulAttack += Mana_SuccesfulAttack;
    }

    public void GainMana(float amount)
    {
        AddValue(amount);
    }

    public void SpendMana(float amount)
    {
        RemoveValue(amount);
    }

    private void Mana_SuccesfulAttack(Collider hit, float damage)
    {
        GainMana(damage / 2);
    }
}
