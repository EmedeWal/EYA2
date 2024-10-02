using UnityEngine.AI;
using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    public CreatureState CurrentState = CreatureState.Idle;

    protected Transform _Transform;

    [Header("CREATURE BASE DATA")]
    [SerializeField] private CreatureData _creatureData;

    private CreatureAnimatorManager _animatorManager;
    private CreatureAttackHandler _attackHandler;
    private NavMeshAgent _agent;
    private Health _health;

    private Transform _chaseTarget;
    private LayerMask _targetLayer;

    public void Init(LayerMask targetLayer)
    {
        _Transform = transform;

        _animatorManager = GetComponent<CreatureAnimatorManager>();
        _attackHandler = GetComponent<CreatureAttackHandler>();
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();

        _targetLayer = targetLayer;

        _animatorManager.Init();
        _attackHandler.Init(_targetLayer);
        _agent.speed = _creatureData.MovementSpeed;
        _health.Init(_creatureData.Health, _creatureData.Health);

        _health.ValueExhausted += CreatureAI_ValueExhausted;
    }

    public void Tick(float delta)
    {
        UpdateStateBehavior();

        float velocity = _agent.velocity.magnitude > 0.1f ? 0 : 1;
        _animatorManager.UpdateAnimatorValues(delta, velocity);
    }

    public void LateTick(float delta)
    {
        _health.LateTick(delta);
    }

    public void Cleanup()
    {
        // Bug here about still trying to access objects?

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

    private void Idle()
    {

    }

    private void Chasing()
    {
        if (TargetInRange())
        {
            CurrentState = CreatureState.Attacking;
        }
    }

    private void Attacking()
    {
        if (_attackHandler.IsAttacking) return;

        if (TargetInRange())
        {
            _attackHandler.StartAttack();
        }
        else
        {
            CurrentState = CreatureState.Chasing;
        }
    }

    private bool TargetInRange()
    {
        Vector3 transformPosition = _Transform.position;
        Vector3 targetPosition = _chaseTarget.position;

        float distance = Vector3.Distance(transformPosition, targetPosition);

        if (distance > _creatureData.ChaseDistance)
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

    private void CreatureAI_ValueExhausted()
    {
        _health.ValueExhausted -= CreatureAI_ValueExhausted;

        Cleanup();
    }
}
