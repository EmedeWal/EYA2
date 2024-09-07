using System.Collections;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    [Header("ATTACK DATA")]
    [SerializeField] private PlayerAttackData _attackData;
    protected bool _CanAttack = true;

    [Header("REFERENCES")]
    [SerializeField] private Transform _attackPoint;
    protected PlayerInputManager _InputManager;
    protected Animator _Animator;
    private CharacterController _controller;
    private PlayerStateManager _stateManager;
    private PlayerDataManager _dataManager;
    private Health _health;

    public delegate void Delegate_SuccessfulAttack(Collider hit, float damage);
    public static event Delegate_SuccessfulAttack SuccessfulAttack;

    [Header("AUDIO")]
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _InputManager = GetComponent<PlayerInputManager>();
        _Animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
        _stateManager = GetComponent<PlayerStateManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _health = GetComponent<Health>();   
    }

    protected void StartCharging()
    {
        CancelInvoke();
        _CanAttack = false;
        _audioSource.clip = _attackData.AttackClip;
        _audioSource.Play();
        _stateManager.SetCharging();
        StartCoroutine(LungeForward());
        Invoke(nameof(StartAttack), _attackData.ChargeDuration);
        Invoke(nameof(ResetAttack), _attackData.AttackSpeed);
        Invoke(nameof(ResetState), _attackData.AttackDuration);
    }

    private void StartAttack()
    {
        Collider[] hits = ColliderRetrieval.CastHitbox(_attackPoint.position, _attackData.AttackSize * 0.5f, _attackPoint.rotation, _attackData.EnemyLayer);

        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent<Health>(out var health)) return;
            OnSuccesfulAttack(hit, _attackData.AttackDamage);
            health.ValueRemoved += PlayerAttack_ValueRemoved;
            health.TakeDamage(_attackData.AttackDamage * _dataManager.GetAttackModifier());
            health.ValueRemoved -= PlayerAttack_ValueRemoved;
        }
    }

    private void ResetLogic()
    {
        CancelInvoke();
        _CanAttack = true;
    }

    private void ResetAttack()
    {
        _stateManager.SetRecharging();
        _CanAttack = true;
    }

    private void ResetState()
    {
        if (_stateManager.IsRecharging())
        {
            _stateManager.SetIdle();
        }
    }

    private void PlayerAttack_ValueRemoved(float amount)
    {
        Debug.Log("Value removed");
        _health.Heal(amount * _dataManager.GetLifeSteal());
    }

    private void OnSuccesfulAttack(Collider hit, float damage)
    {
        SuccessfulAttack?.Invoke(hit, damage);
    }

    private IEnumerator LungeForward()
    {
        // Wait a little bit so the lunge ends exactly when the player attacks
        yield return new WaitForSeconds(_attackData.ChargeDuration - _attackData.LungeDuration);
        Collider[] enemies = ColliderRetrieval.CastHitbox(_attackPoint.position, _attackData.AttackSize * 0.5f, _attackPoint.rotation, _attackData.EnemyLayer);
        _stateManager.SetAttacking();

        if (enemies.Length == 0)
        {
            float startTime = Time.time;
            while (Time.time < startTime + _attackData.LungeDuration)
            {
                _controller.Move(_attackData.LungeSpeed * Time.deltaTime * transform.forward);
                yield return null;
            }
        }
    }
}
