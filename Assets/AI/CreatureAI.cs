using UnityEngine.AI;
using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    [Header("CREATURE BASE DATA")]
    public CreatureData CreatureData;

    public CreatureState CurrentState = CreatureState.Idle;

    protected Transform _Transform;

    private CreatureAnimatorManager _animatorManager;
    private CreatureAttackHandler _attackHandler;
    private NavMeshAgent _agent;
    private Health _health;

    private Transform _chaseTarget;
    private LayerMask _targetLayer;

    public virtual void Init(LayerMask targetLayer)
    {
        _Transform = transform;

        _animatorManager = GetComponent<CreatureAnimatorManager>();
        _attackHandler = GetComponent<CreatureAttackHandler>();
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();

        _attackHandler.DamageModifier = CreatureData.DamageModifier;

        _targetLayer = targetLayer;

        _animatorManager.Init();
        _attackHandler.Init(_targetLayer);
        _agent.speed = CreatureData.MovementSpeed;
        _health.Init(CreatureData.Health, CreatureData.Health);
    }

    public virtual void Tick(float delta)
    {
        UpdateStateBehavior();

        float velocity = _agent.velocity.magnitude > 0.1f ? 1 : 0;
        _animatorManager.UpdateAnimatorValues(delta, velocity);
    }

    public virtual void LateTick(float delta)
    {
        _health.LateTick(delta);
    }

    public virtual void Cleanup()
    {
        Destroy(gameObject);
    }

    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
        CurrentState = CreatureState.Chasing;
    }

    private void UpdateStateBehavior()
    {
        switch (CurrentState)
        {
            case CreatureState.Idle:
                Idle();
                break;

            case CreatureState.Chasing:
                Chasing();
                break;

            case CreatureState.Attacking:
                Attacking();
                break;
        }
    }

    protected virtual void Idle()
    {
        _agent.SetDestination(_Transform.position);

        Collider[] hitColliders = Physics.OverlapSphere(_Transform.position, CreatureData.DetectionDistance, _targetLayer);
        if (hitColliders.Length > 0)
        {
            Transform nearestTarget = GetNearestTarget(hitColliders);

            if (nearestTarget != null)
            {
                SetChaseTarget(nearestTarget);
            }
        }
    }

    protected virtual void Chasing()
    {
        if (_chaseTarget == null)
        {
            CurrentState = CreatureState.Idle;
        }
        else if (TargetInRange())
        {
            CurrentState = CreatureState.Attacking;
        }
    }

    protected virtual void Attacking()
    {
        if (_chaseTarget == null)
        {
            CurrentState = CreatureState.Idle;
        }
        else if (_attackHandler.IsAttacking) return;

        if (TargetInRange())
        {
            _attackHandler.StartAttack();
        }
        else
        {
            CurrentState = CreatureState.Chasing;
        }
    }

    private Transform GetNearestTarget(Collider[] hitColliders)
    {
        Transform nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(_Transform.position, hitCollider.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = hitCollider.transform;
            }
        }

        return nearestTarget;
    }

    private bool TargetInRange()
    {
        Vector3 transformPosition = _Transform.position;
        Vector3 targetPosition = _chaseTarget.position;

        transformPosition.y = 0;
        targetPosition.y = 0;

        float distance = Vector3.Distance(transformPosition, targetPosition);

        if (distance > CreatureData.ChaseDistance)
        {
            _agent.SetDestination(targetPosition);
            return false;
        }
        else
        {
            _agent.SetDestination(transformPosition);
            return true;
        }
    }
}
