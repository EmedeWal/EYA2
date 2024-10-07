using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mastery Perk", menuName = "Scriptable Object/Perks/Passive Perk/Mastery")]
public class MasteryPerk : PerkData
{
    private PlayerAttackHandler _playerAttackHandler;
    private Health _playerHealth;

    private LayerMask _targetLayer;

    private VFXManager _VFXManager;

    public override void Init(GameObject playerObject, List<PerkData> perks = null)
    {
        base.Init(playerObject, perks);

        _playerAttackHandler = _PlayerObject.GetComponent<PlayerAttackHandler>();
        _playerHealth = _PlayerObject.GetComponent<Health>();

        _targetLayer = LayerMask.GetMask("DamageCollider");

        _VFXManager = VFXManager.Instance;
    }

    public override void Activate()
    {
        _playerAttackHandler.SuccessfulHit += MasteryPerk_SuccesfulHit;
    }

    public override void Deactivate()
    {
        _playerAttackHandler.SuccessfulHit -= MasteryPerk_SuccesfulHit;
    }

    public override void Tick(float delta)
    {
        // Logic for any effects that need to be updated over time
    }

    private void MasteryPerk_SuccesfulHit(Collider hit, float damage, bool crit)
    {

    }
}