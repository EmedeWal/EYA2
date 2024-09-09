using System.Collections;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    private Transform _transform;
    private float _delta;

    [Header("ATTACK DATA")]
    [SerializeField] private PlayerAttackData _lightAttackData;
    [SerializeField] private PlayerAttackData _heavyAttackData;

    [Header("REFERENCES")]
    [SerializeField] private Transform _attackPoint;
    private PlayerAnimatorManager _animatorManager;
    private PlayerAudioManager _audioManager;
    private PlayerInputHandler _inputHandler;
    private PlayerDataManager _dataManager;
    private PlayerLocomotion _locomotion;
    private Health _health;
    private LayerMask _damageCollider;
    private bool _isAttacking = false;

    public delegate void Delegate_SuccessfulAttack(Collider hit, float damage);
    public event Delegate_SuccessfulAttack SuccessfulAttack;


    private void Awake()
    {
        _transform = transform;
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _audioManager = GetComponent<PlayerAudioManager>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _dataManager = GetComponent<PlayerDataManager>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _health = GetComponent<Health>();

        _damageCollider = LayerMask.GetMask("DamageCollider");
    }

    private void OnEnable()
    {
        _inputHandler.LightAttackInputPerformed += PlayerAttackHandler_LightAttackInputPerformed;
        _inputHandler.HeavyAttackInputPerformed += PlayerAttackHandler_HeavyAttackInputPerformed;
    }

    private void OnDisable()
    {
        _inputHandler.LightAttackInputPerformed -= PlayerAttackHandler_LightAttackInputPerformed;
        _inputHandler.HeavyAttackInputPerformed -= PlayerAttackHandler_HeavyAttackInputPerformed;
    }

    private void PlayerAttackHandler_LightAttackInputPerformed()
    {
        PrepareAttack(_lightAttackData);
    }

    private void PlayerAttackHandler_HeavyAttackInputPerformed()
    {
        PrepareAttack(_heavyAttackData);
    }

    public void OnUpdate(float delta)
    {
        _delta = delta;
    }

    private void PrepareAttack(PlayerAttackData attackData)
    {
        if (_animatorManager.GetBool("InAction") || _isAttacking) return; _isAttacking = true;
        _animatorManager.CrossFadeAnimation(_delta, attackData.AnimationName, 0.1f, 1);
        _audioManager.PlayAudioClip(_audioManager._AudioSources[1], attackData.AttackClip, 0);
        StartCoroutine(Attack(attackData));
    }

    private IEnumerator Attack(PlayerAttackData attackData)
    {
        float animationDuration = _animatorManager._Animator.GetCurrentAnimatorStateInfo(1).length;
        float lungeDelay = animationDuration / 8f;
        float attackDelay = animationDuration / 4f;

        yield return new WaitForSeconds(lungeDelay);

        _locomotion.AddForce(_transform.forward * attackData.LungeForce);

        yield return new WaitForSeconds(attackDelay);

        _locomotion.RemoveForce();

        Collider[] hits = ColliderRetrieval.CastHitbox(_attackPoint.position, attackData.AttackSize * 0.5f, _attackPoint.rotation, _damageCollider);

        foreach (Collider hit in hits)
        {
            Debug.Log($"{hit.name} detected.");

            if (hit.TryGetComponent<Health>(out var health))
            {
                OnSuccesfulAttack(hit, attackData.AttackDamage);
                health.ValueRemoved += PlayerAttack_ValueRemoved;
                health.TakeDamage(attackData.AttackDamage * _dataManager.GetAttackModifier());
                health.ValueRemoved -= PlayerAttack_ValueRemoved;
            }
        }

        _isAttacking = false;
    }


    private void PlayerAttack_ValueRemoved(float amount)
    {
        Debug.Log($"{amount} damage dealt.");
        _health.Heal(amount * _dataManager.GetLifeSteal());
    }

    private void OnSuccesfulAttack(Collider hit, float damage)
    {
        SuccessfulAttack?.Invoke(hit, damage);
    }
}
