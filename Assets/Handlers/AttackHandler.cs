using UnityEngine;

public abstract class AttackHandler : MonoBehaviour
{
    [HideInInspector] public bool IsAttacking = false;

    [Header("AUDIO SOURCE")]
    [SerializeField] private AudioSource _audioSource;

    // Inherited properties
    protected AnimatorManager _AnimatorManager;
    protected AudioSystem _AudioSystem;
    protected AttackData _AttackData;
    protected Transform _Transform;
    private LayerMask _TargetLayer;
    protected float _Delta;

    public delegate void Delegate_SuccessfulAttack(Collider hit, float damage, bool crit);
    public event Delegate_SuccessfulAttack SuccessfulAttack;

    public virtual void Init(LayerMask targetLayer)
    {
        _AnimatorManager = GetComponent<AnimatorManager>();
        _AudioSystem = AudioSystem.Instance;
        _TargetLayer = targetLayer;
        _Transform = transform;
    }

    public void Tick(float delta)
    {
        _Delta = delta;
    }

    public void Attack()
    {
        float damage = _AttackData.Damage;
        bool crit = RollCritical();

        HandleDamage(damage, crit);
    }

    protected void HandleAttack(AttackData attackData)
    {
        if (_AnimatorManager.GetBool("InAction") || IsAttacking) return;
        {
            IsAttacking = true;
            _AttackData = attackData;
            _AudioSystem.PlayAudioClip(_audioSource, _AttackData.AttackClip);
            _AnimatorManager.CrossFadeAnimation(_Delta, _AttackData.AnimationName, 0.1f, 1);
        }
    }

    protected virtual void HandleDamage(float damage, bool crit)
    {
        Vector3 attackPosition = _Transform.position + _Transform.TransformDirection(_AttackData.AttackOffset);

        Collider[] hits = Physics.OverlapSphere(attackPosition, _AttackData.AttackRadius, _TargetLayer);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                OnSuccesfulAttack(hit, damage, crit);
                health.TakeDamage(gameObject, damage);
            }
        }

        AttackEnd();
    }

    protected abstract void AttackEnd();

    protected abstract bool RollCritical();

    private void OnSuccesfulAttack(Collider hit, float damage, bool crit)
    {
        SuccessfulAttack?.Invoke(hit, damage, crit);
    }
}
