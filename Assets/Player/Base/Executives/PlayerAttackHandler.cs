using System.Collections;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    private Transform _transform;
    private float _delta;

    [Header("PLAYER STATS")]
    [SerializeField] private PlayerStats _stats;

    [Header("ATTACK DATA")]
    [SerializeField] private PlayerAttackData _lightAttackData;
    [SerializeField] private PlayerAttackData _heavyAttackData;

    [Header("REFERENCES")]
    [SerializeField] private Transform _attackPoint;
    private PlayerInputHandler _inputHandler;
    private PlayerAnimatorManager _animatorManager;
    private PlayerDataManager _dataManager;
    private PlayerLocomotion _locomotion;
    private Health _health;
    private LayerMask _damageCollider;
    private bool _isAttacking = false;

    private AudioSystem _audioSystem;

    public delegate void Delegate_SuccessfulAttack(Collider hit, float damage);
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

    private void PlayerAttackHandler_LightAttackInputPerformed()
    {
        if (_animatorManager.GetBool("InAction") || _isAttacking) return; _isAttacking = true;
        PrepareAttack(_lightAttackData, _stats.GetStat(Stat.LightAttackDamage));
    }

    private void PlayerAttackHandler_HeavyAttackInputPerformed()
    {
        if (_animatorManager.GetBool("InAction") || _isAttacking) return; _isAttacking = true;
        PrepareAttack(_heavyAttackData, _stats.GetStat(Stat.HeavyAttackDamage));
    }

    private void PrepareAttack(PlayerAttackData attackData, float finalDamage)
    {
        _animatorManager.CrossFadeAnimation(_delta, attackData.AnimationName, 0.1f, 1);
        _audioSystem.PlayAudioClip(_dataManager.ReferenceStruct.AttackSource, attackData.AttackClip);
        StartCoroutine(Attack(attackData, finalDamage));
    }

    private IEnumerator Attack(PlayerAttackData attackData, float finalDamage)
    {
        float animationDuration = _animatorManager._Animator.GetCurrentAnimatorStateInfo(1).length;
        float lungeDelay = animationDuration / 8f;
        float attackDelay = animationDuration / 4f;

        yield return new WaitForSeconds(lungeDelay);

        _locomotion.AddForce(_transform.forward * attackData.LungeForce);

        yield return new WaitForSeconds(attackDelay);

        _locomotion.RemoveForce();

        Collider[] hits = Helpers.CastHitBox(_attackPoint.position, attackData.AttackSize * 0.5f, _attackPoint.rotation, _damageCollider);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                OnSuccesfulAttack(hit, finalDamage);
                float damage = health.TakeDamage(finalDamage);
            }
        }

        _isAttacking = false;
    }

    private void OnSuccesfulAttack(Collider hit, float damage)
    {
        SuccessfulAttack?.Invoke(hit, damage);
    }
}
