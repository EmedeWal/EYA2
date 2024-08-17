using UnityEngine.AI;
using UnityEngine;

public abstract class EnemyAI : Enemy
{
    [Header("STATE MACHINE")]
    public EnemyState EnemyState = EnemyState.Chasing;

    [Header("REFERENCES")]
    [SerializeField] protected Animator Animator;
    [SerializeField] protected LayerMask PlayerLayer;

    protected NavMeshAgent Agent;
    protected AudioSource AudioSource;
    protected Transform PlayerTransform;

    [Header("VARIABLES")]
    [SerializeField] protected float RotationSpeed;
    [SerializeField] protected float RotationModifier;

    protected void SetReferences()
    {
        Agent = GetComponent<NavMeshAgent>();
        AudioSource = GetComponent<AudioSource>();
        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    protected void CancelMovement()
    {
        Agent.SetDestination(transform.position);
        Animator.SetFloat("Speed", 0f);
    }

    protected virtual void MoveTowardsPlayer()
    {
        Agent.SetDestination(PlayerTransform.position);
        Animator.SetFloat("Speed", Agent.velocity.magnitude);
    }

    protected void RotateTowardsPlayer()
    {
        Vector3 direction = (PlayerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    protected void PlayAnimation(string animationParameter)
    {
        Animator.SetTrigger(animationParameter);
    }

    protected void PlayAudio(AudioClip audioClip)
    {
        AudioSource.clip = audioClip;
        AudioSource.Play();
    }

    protected bool InRange(float range)
    {
        return Vector3.Distance(PlayerTransform.position, transform.position) <= range;
    }

    private void SetState(EnemyState newState)
    {
        EnemyState = newState;
    }

    public void SetIdle()
    {
        SetState(EnemyState.Idle);
        StopAllCoroutines();
        CancelInvoke();
    }

    protected void SetChasing()
    {
        SetState(EnemyState.Chasing);
    }

    protected void SetCharging()
    {
        SetState(EnemyState.Charging);
    }

    protected void SetAttacking()
    {
        SetState(EnemyState.Attacking);
    }

    protected void SetFiring()
    {
        SetState(EnemyState.Firing);
    }

    protected void SetRetreating()
    {
        SetState(EnemyState.Retreating);
    }
}


