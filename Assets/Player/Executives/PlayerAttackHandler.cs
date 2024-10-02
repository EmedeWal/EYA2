using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    private Transform _transform;
    private float _delta;

    [Header("PLAYER STATS")]
    [SerializeField] private PlayerStats _stats;

    [Header("ATTACK DATA")]
    [SerializeField] private AttackData _lightAttackData;
    [SerializeField] private AttackData _heavyAttackData;
    private AttackData _currentAttackData;
    private float _currentAttackDamage;

    // Loads of references lol
    private PlayerInputHandler _inputHandler;
    private PlayerAnimatorManager _animatorManager;
    private PlayerDataManager _dataManager;
    private PlayerLocomotion _locomotion;
    private Health _health;
    private LayerMask _damageCollider;
    private bool _isAttacking = false;

    private AudioSystem _audioSystem;

    public delegate void Delegate_SuccessfulAttack(Collider hit, float damage, bool critical);
    public event Delegate_SuccessfulAttack SuccessfulAttack;

    public void Init()
    {
        _transform = transform;

        _inputHandler = GetComponent<PlayerInputHandler>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _health = GetComponent<Health>();

        _damageCollider = LayerMask.GetMask("DamageCollider");

        _audioSystem = AudioSystem.Instance;

        _inputHandler.LightAttackInputPerformed += PlayerAttackHandler_LightAttackInputPerformed;
        _inputHandler.HeavyAttackInputPerformed += PlayerAttackHandler_HeavyAttackInputPerformed;
    }

    public void Tick(float delta)
    {
        _delta = delta;
    }

    public void Cleanup()
    {
        _inputHandler.LightAttackInputPerformed -= PlayerAttackHandler_LightAttackInputPerformed;
        _inputHandler.HeavyAttackInputPerformed -= PlayerAttackHandler_HeavyAttackInputPerformed;
    }

    public void Attack()
    {
        bool critical;

        if (Helpers.GetChanceRoll(_stats.GetCurrentStat(Stat.CriticalChance)))
        {
            critical = true;
        }
        else
        {
            critical = false;
        }

        if (critical)
        {
            _currentAttackDamage *= _stats.GetCurrentStat(Stat.CriticalMultiplier);
        }

        Vector3 attackPosition = _transform.position + _transform.TransformDirection(_currentAttackData.AttackOffset);

        Collider[] hits = Physics.OverlapSphere(attackPosition, _currentAttackData.AttackRadius, _damageCollider);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                OnSuccesfulAttack(hit, _currentAttackDamage, critical);
                health.TakeDamage(gameObject, _currentAttackDamage);
            }
        }

        _isAttacking = false;
    }

    private void PlayerAttackHandler_LightAttackInputPerformed()
    {
        if (_animatorManager.GetBool("InAction") || _isAttacking) return;
        {
            _isAttacking = true;
            _currentAttackData = _lightAttackData;
            _currentAttackDamage = _stats.GetCurrentStat(Stat.LightAttackDamage);
            PrepareAttack();
        }
    }

    private void PlayerAttackHandler_HeavyAttackInputPerformed()
    {
        if (_animatorManager.GetBool("InAction") || _isAttacking) return;
        {
            _isAttacking = true;
            _currentAttackData = _heavyAttackData;
            _currentAttackDamage = _stats.GetCurrentStat(Stat.HeavyAttackDamage);
            PrepareAttack();
        }
    }

    private void PrepareAttack()
    {
        _animatorManager.CrossFadeAnimation(_delta, _currentAttackData.AnimationName, 0.1f, 1);
        _audioSystem.PlayAudioClip(_dataManager.ReferenceStruct.AttackSource, _currentAttackData.AttackClip);
    }

    private void OnSuccesfulAttack(Collider hit, float damage, bool critical)
    {
        SuccessfulAttack?.Invoke(hit, damage, critical);
    }
}
