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
        PrepareAttack(_lightAttackData);
    }

    private void PlayerAttackHandler_HeavyAttackInputPerformed()
    {
        PrepareAttack(_heavyAttackData);
    }

    private void PrepareAttack(PlayerAttackData attackData)
    {
        if (_animatorManager.GetBool("InAction") || _isAttacking) return; _isAttacking = true;
        _animatorManager.CrossFadeAnimation(_delta, attackData.AnimationName, 0.1f, 1);
        _audioSystem.PlayAudioClip(_dataManager.ReferenceStruct.AttackSource, attackData.AttackClip);
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

        Collider[] hits = Helpers.CastHitBox(_attackPoint.position, attackData.AttackSize * 0.5f, _attackPoint.rotation, _damageCollider);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                OnSuccesfulAttack(hit, attackData.AttackDamage);
                _health.Heal(health.TakeDamage(attackData.AttackDamage * _dataManager.AttackStruct.AttackModifier) * _dataManager.StanceStruct.LifeSteal);
            }
        }

        _isAttacking = false;
    }

    private void OnSuccesfulAttack(Collider hit, float damage)
    {
        SuccessfulAttack?.Invoke(hit, damage);
    }
}
