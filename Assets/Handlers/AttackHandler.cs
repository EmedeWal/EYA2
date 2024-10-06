using UnityEngine;

public abstract class AttackHandler : MonoBehaviour
{
    [HideInInspector] public bool IsAttacking = false;


    [Header("CRIT EFFECTS")]
    [SerializeField] private VFX _critVFX;

    [Header("AUDIO SOURCE")]
    [SerializeField] private AudioSource _audioSource;

    // Inherited properties
    protected AnimatorManager _AnimatorManager;
    protected AudioSystem _AudioSystem;
    protected VFXManager _VFXManager;
    protected AttackData _AttackData;
    protected Transform _Transform;
    protected float _Delta;

    private LayerMask _targetLayer;

    public delegate void Delegate_SuccessfulAttack(Collider hit, float damage, bool crit);
    public event Delegate_SuccessfulAttack SuccessfulAttack;

    public virtual void Init(LayerMask targetLayer)
    {
        _AnimatorManager = GetComponent<AnimatorManager>();
        _AudioSystem = AudioSystem.Instance;
        _VFXManager = VFXManager.Instance;
        _targetLayer = targetLayer;
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
        IsAttacking = true;
        _AttackData = attackData;
        _AnimatorManager.CrossFadeAnimation(_Delta, _AttackData.AnimationName, 0.1f, 1);
        _AudioSystem.PlayAudioClip(_audioSource, _AttackData.AudioClip, _AttackData.AudioVolume, _AttackData.AudioOffset);
    }

    protected virtual void HandleDamage(float damage, bool crit)
    {
        Vector3 attackPosition = _Transform.position + _Transform.TransformDirection(_AttackData.AttackOffset);

        Collider[] hits = Physics.OverlapSphere(attackPosition, _AttackData.AttackRadius, _targetLayer);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                if (crit && hit.TryGetComponent(out LockTarget lockTarget))
                {
                    Transform center = lockTarget.Center;
                    VFX critVFX = Instantiate(_critVFX, center.position, center.rotation);
                    AudioSource source = critVFX.GetComponent<AudioSource>();
                    _AudioSystem.PlayAudioClip(source, source.clip, source.volume);
                    _VFXManager.AddVFX(critVFX, center, true, 1f);
                }

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
