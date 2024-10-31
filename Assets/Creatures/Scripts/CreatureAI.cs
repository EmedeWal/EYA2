using System.Collections.Generic;
using UnityEngine;

// Uncomment this when making new creatures for quick setup
//[RequireComponent (typeof(CreatureAnimatorManager))]
//[RequireComponent (typeof(CreatureAttackHandler))]
//[RequireComponent (typeof(CreatureStatManager))]
//[RequireComponent (typeof(CreatureLocomotion))]
//[RequireComponent (typeof(CreatureHealth))]
//[RequireComponent (typeof(BleedHandler))]
//[RequireComponent (typeof(LockTarget))]
public class CreatureAI : MonoBehaviour
{
    private CreatureState _currentState;

    [Header("CREATURE BASE DATA")]
    public CreatureData CreatureData;

    [HideInInspector] public Transform DefaultTarget;
    [HideInInspector] public Transform CurrentTarget;
    public CreatureAnimatorManager AnimatorManager { get; private set; }
    public CreatureAttackHandler AttackHandler { get; private set; }
    public CreatureStatManager StatManager { get; private set; }
    public CreatureLocomotion Locomotion { get; private set; }
    public BleedHandler BleedHandler { get; private set; }
    public LockTarget LockTarget { get; private set; }
    public Transform Transform { get; private set; }
    public Health Health { get; private set; }
    public LayerMask TargetLayer { get; private set; }

    public virtual void Init(LayerMask creatureLayer, LayerMask targetLayer, Transform defaultTarget = null)
    {
        Transform = transform;

        gameObject.layer = Mathf.RoundToInt(Mathf.Log(creatureLayer.value, 2));
        DefaultTarget = defaultTarget;
        TargetLayer = targetLayer;

        AnimatorManager = GetComponent<CreatureAnimatorManager>();
        AttackHandler = GetComponent<CreatureAttackHandler>();
        StatManager = GetComponent<CreatureStatManager>();
        Locomotion = GetComponent<CreatureLocomotion>();
        BleedHandler = GetComponent<BleedHandler>();
        LockTarget = GetComponent<LockTarget>();
        Health = GetComponent<Health>();

        AnimatorManager.Init();
        AttackHandler.Init(TargetLayer);
        StatManager.Init();
        Locomotion.Init(CreatureData.RunSpeed);
        BleedHandler.Init();
        LockTarget.Init();
        Health.Init(CreatureData.Health, CreatureData.Health);

        if (TryGetComponent(out FootstepHandler footstepHandler)) footstepHandler.Init();

        AnimatorManager.SpawnAnimationFinished += CreatureAI_SpawnAnimationFinished;

        CurrentTarget = DefaultTarget;
    }

    public virtual void Tick(float delta)
    {
        float locomotionValue = Locomotion.GetLocomotionValue();
        AnimatorManager.Tick(delta, locomotionValue);   
        //_currentState?.Tick(delta);
    }

    public virtual void LateTick(float delta)
    {
        Health.LateTick(delta);
    }

    public virtual void Cleanup()
    {
        AnimatorManager.SpawnAnimationFinished -= CreatureAI_SpawnAnimationFinished;

        BleedHandler.Cleanup();

        Destroy(Locomotion);
        Destroy(BleedHandler);
        Destroy(LockTarget);
        Destroy(Health);
    }

    public virtual void DetermineBehavior(AttackData attackData, Transform target)
    {
        DetermineAttack(target);
    }

    public virtual void DetermineAttack(Transform target)
    {
        CurrentTarget = target;

        List<AttackData> viableAttacks = new();

        if (IsTargetInFront(CurrentTarget))
        {
            viableAttacks.AddRange(AttackHandler.AttackDataList.FindAll(a => a.AttackMode == AttackMode.Lunging || a.AttackMode == AttackMode.Tracking));
        }
        else
        {
            viableAttacks.AddRange(AttackHandler.AttackDataList.FindAll(a => a.AttackMode == AttackMode.Tracking));
        }

        AttackHandler.SelectRandomAttack(viableAttacks);
    }

    public void SetState(CreatureState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void CreatureAI_SpawnAnimationFinished()
    {
        SetState(new IdleState(this));
    }

    public Transform GetNearestTarget(float maxDistance)
    {
        Collider[] hitColliders = Physics.OverlapSphere(Transform.position, maxDistance, TargetLayer);
        Transform nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        if (hitColliders.Length > 0)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                float distance = Vector3.Distance(Transform.position, hitCollider.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestTarget = hitCollider.transform;
                }
            }
        }

        return nearestTarget;
    }

    public float GetDistanceToTarget(Transform target)
    {
        return Vector3.Distance(Transform.position, target.position);
    }

    public bool IsTargetInRange(Transform target)
    {
        float threshold = CreatureData.AttackDistance + AttackHandler.AttackData.Distance;
        float distance = GetDistanceToTarget(target);
        return distance <= threshold;
    }

    public bool IsTargetInFront(Transform target)
    {
        Vector3 directionToTarget = (target.position - Transform.position).normalized;
        float angleToTarget = Vector3.Angle(Transform.forward, directionToTarget);
        return angleToTarget < CreatureData.MaxAngle;
    }

    public bool IsTargetBehind(Transform target)
    {
        Vector3 directionToTarget = (target.position - Transform.position).normalized;
        float angleToTarget = Vector3.Angle(Transform.forward, directionToTarget);
        return angleToTarget > 90;
    }
}