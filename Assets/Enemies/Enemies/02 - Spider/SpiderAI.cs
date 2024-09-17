using System.Collections;
using UnityEngine;

public class SpiderAI : MeleeEnemyAI
{
    [Header("ATTACK DATA")]
    [SerializeField] private EnemyMeleeData _lungeData;
    [SerializeField] private EnemyMeleeData _flurryData;
    private int _attackNumber;

    [Header("FLURRY ATTACK")]
    [SerializeField] private int _flurryAttackAmount = 4;
    [SerializeField] private float _flurryAttackOffset = 0.3f;

    [Header("MOVEMENT INTERCEPTION")]
    [SerializeField] private float _predictionThreshold = 5f;
    private MovementTracking _movementTracking;
    private Vector3 _expectedPosition;

    private void Awake()
    {
        DetermineAttackData();
        SetReferences();

        _movementTracking = PlayerTransform.gameObject.GetComponent<MovementTracking>();
    }

    protected override void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector3.Distance(PlayerTransform.position, transform.position);
        float timeToPlayer = distanceToPlayer / Agent.speed;
        Vector3 targetPosition = PlayerTransform.position + _movementTracking.AverageVelocity * timeToPlayer;

        if (distanceToPlayer < _predictionThreshold)
        {
            targetPosition = PlayerTransform.position;
        }

        _expectedPosition = targetPosition;

        Agent.SetDestination(targetPosition);
        Animator.SetFloat("Speed", Agent.velocity.magnitude);
    }

    protected override void DetermineAttackData()
    {
        _attackNumber = Random.Range(0, 2);

        if (_attackNumber == 0)
        {
            SetAttackData(_lungeData);
        }
        else
        {
            SetAttackData(_flurryData);
        }
    }

    protected override void AttackAction()
    {
        if (_attackNumber == 0)
        {
            LungeAttack();
        }
        else
        {
            FlurryAttack();
        }
    }

    private void LungeAttack()
    {
        DealDamage(CastBox());
    }

    private void FlurryAttack()
    {
        StartCoroutine(FlurryAttackCoroutine());
    }

    private IEnumerator FlurryAttackCoroutine()
    {
        for (int i = 0; i < _flurryAttackAmount; i++)
        {
            DealDamage(CastBox());

            yield return new WaitForSeconds(_flurryAttackOffset);
        }
    }

    void OnDrawGizmos()
    {
        if (Agent != null && Agent.isActiveAndEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _expectedPosition);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_expectedPosition, 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Agent.destination);
        }
    }
}
