using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    private CreatureState _currentState;

    [Header("CREATURE BASE DATA")]
    public CreatureData CreatureData;

    [HideInInspector] public Transform DefaultTarget;
    public CreatureAnimatorManager AnimatorManager { get; private set; }
    public CreatureAttackHandler AttackHandler { get; private set; }
    public CreatureStatManager StatManager { get; private set; }
    public CreatureLocomotion Locomotion { get; private set; }
    public BleedHandler BleedHandler { get; private set; }
    public LockTarget LockTarget { get; private set; }
    public Transform Transform { get; private set; }
    public Health Health { get; private set; }
    public LayerMask TargetLayer { get; private set; }

    public void Init(LayerMask creatureLayer, LayerMask targetLayer, Transform defaultTarget = null)
    {
        Transform = transform;

        gameObject.layer = Mathf.RoundToInt(Mathf.Log(creatureLayer.value, 2));
        TargetLayer = targetLayer;
        DefaultTarget = defaultTarget;

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

        SetState(new IdleState(this));
    }

    public void Tick(float delta)
    {
        AnimatorManager.Tick(delta, Locomotion.GetLocomotionValue());
        _currentState?.Tick(delta);
    }

    public void LateTick(float delta)
    {
        Health.LateTick(delta);
    }

    public void Cleanup()
    { 
        _currentState?.Exit();

        BleedHandler.Cleanup();

        Destroy(Locomotion);
        Destroy(BleedHandler);
        Destroy(LockTarget);
        Destroy(Health);
    }

    public void SetState(CreatureState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
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

    public bool TargetInRange(Transform target)
    {
        float threshold = CreatureData.AttackDistance + AttackHandler.AttackData.Distance;
        float distance = Vector3.Distance(Transform.position, target.position);
        return distance <= threshold;
    }
}