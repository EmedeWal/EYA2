using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    private Transform _transform;

    [Header("CREATURE BASE DATA")]
    public CreatureData CreatureData;
    private CreatureState _currentState;

    public CreatureAnimatorManager AnimatorManager { get; private set; }
    public CreatureAttackHandler AttackHandler { get; private set; }
    public CreatureLocomotion Locomotion { get; private set; }
    public LockTarget LockTarget { get; private set; }
    public Health Health { get; private set; }
    public LayerMask TargetLayer { get; private set; }

    public void Init(LayerMask creatureLayer, LayerMask targetLayer)
    {
        _transform = transform;

        gameObject.layer = Mathf.RoundToInt(Mathf.Log(creatureLayer.value, 2));
        TargetLayer = targetLayer;

        AnimatorManager = GetComponent<CreatureAnimatorManager>();
        AttackHandler = GetComponent<CreatureAttackHandler>();
        Locomotion = GetComponent<CreatureLocomotion>();
        LockTarget = GetComponent<LockTarget>();
        Health = GetComponent<Health>();

        AnimatorManager.InitCreature(Health);
        AttackHandler.Init(TargetLayer);
        Locomotion.Init(CreatureData.RunSpeed);
        LockTarget.Init();
        Health.Init(CreatureData.Health, CreatureData.Health);

        if (TryGetComponent(out FootstepHandler footstepHandler)) footstepHandler.Init();

        SetState(new IdleState(this));
    }

    public void Cleanup()
    {
        AnimatorManager.CleanupCreature(Health);

        _currentState?.Exit();
    }

    public void SetState(CreatureState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Tick(float delta)
    {
        AnimatorManager.Tick(delta, Locomotion.GetLocomotionValue());
        _currentState.Tick(delta);
    }

    public void LateTick(float delta)
    {
        Health.LateTick(delta);
    }

    public Transform GetNearestTarget(Collider[] hitColliders)
    {
        Transform nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(_transform.position, hitCollider.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = hitCollider.transform;
            }
        }

        return nearestTarget;
    }

    public bool TargetInRange(Transform target)
    {
        float distance = Vector3.Distance(_transform.position, target.position);
        return distance <= CreatureData.AttackDistance;
    }
}