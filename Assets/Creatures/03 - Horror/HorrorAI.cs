using System.Collections.Generic;
using UnityEngine;

public class HorrorAI : CreatureAI
{
    [Header("TELEPORTATION")]
    [SerializeField] private TeleportAttackData _teleportAttackData;

    [Header("PROJECTILE")]
    [SerializeField] private ProjectileAttackData _projectileAttackDataList;
    [SerializeField] private ThrowAttackData _throwAttackData;
    [SerializeField] private GrabAttackData _grabAttackData;
    [SerializeField] private Transform _grabOrigin;
    [SerializeField] private Transform _firePoint;

    [Header("PLAYER STATS")]
    [SerializeField] private PlayerStats _playerStats;

    public override void Init(LayerMask creatureLayer, LayerMask targetLayer, Transform defaultTarget = null)
    {
        base.Init(creatureLayer, targetLayer, defaultTarget);

        _grabAttackData.Init(_playerStats, _grabOrigin, Health, targetLayer);
        _projectileAttackDataList.Init(_firePoint, targetLayer);
        _throwAttackData.Init(_firePoint, targetLayer);
    }

    public override void DetermineBehavior(BaseAttackData attackData, Transform target)
    {
        if (attackData.AttackType == AttackType.Heavy && IsTargetBehind(target))
        {
            Teleport();
        }
        else if (!IsTargetInFront(target) && IsTargetInRange(target))
        {
            SetState(new CirclingState(this, target));
        }
        else
        {
            DetermineAttack(target);
        }
    }

    public override void DetermineAttack(Transform target)
    {
        CurrentTarget = target;

        List<BaseAttackData> viableAttacks = new();

        float distance = GetDistanceToTarget(target);
        float attackRange = CreatureData.AttackDistance;
        bool isTargetInRange = distance <= attackRange;

        if (IsTargetInFront(CurrentTarget))
        {
            viableAttacks.AddRange(
                AttackHandler.AttackDataList.FindAll(a =>
                    a.AttackMode == AttackMode.Lunging || a.AttackMode == AttackMode.Tracking
                )
            );
        }
        else
        {
            viableAttacks.AddRange(
                AttackHandler.AttackDataList.FindAll(a =>
                    a.AttackMode == AttackMode.Tracking
                )
            );
        }

        AttackHandler.SelectRandomAttack(viableAttacks);
    }


    private void Teleport()
    {
        _teleportAttackData.Init(this);
        AttackHandler.SelectAttack(_teleportAttackData);

        SetState(new AttackingState(this, CurrentTarget));
    }
}