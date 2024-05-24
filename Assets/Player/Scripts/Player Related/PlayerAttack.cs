using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    [Header("ATTACK DATA")]
    [SerializeField] private PlayerAttackData _attackData;
    protected bool CanAttack = true;

    [Header("REFERENCES")]
    [SerializeField] private Transform _attackPoint;
    private PlayerStateManager _stateManager;
    private CharacterController _controller;

    protected PlayerInputManager InputManager;
    protected PlayerDataManager  DataManager;
    protected Animator Animator;

    public delegate void PlayerAttack_SuccesfulAttack(Collider hit, float damage);
    public static event PlayerAttack_SuccesfulAttack SuccesfulAttack;

    [Header("AUDIO")]
    [SerializeField] private AudioSource _audioSource;

    protected virtual void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _controller = GetComponent<CharacterController>();
        InputManager = GetComponent<PlayerInputManager>();
        DataManager = GetComponent<PlayerDataManager>();
        Animator = GetComponentInChildren<Animator>();
    }

    protected virtual void ResetLogic()
    {
        CancelInvoke();
        CanAttack = true;
    }

    protected virtual void StartCharging()
    {
        CancelInvoke();

        CanAttack = false;

        _audioSource.clip = _attackData.AttackClip;
        _audioSource.Play();

        _stateManager.SetCharging();

        StartCoroutine(LungeForward());
        Invoke(nameof(StartAttack), _attackData.ChargeDuration);
        Invoke(nameof(ResetAttack), _attackData.AttackSpeed);
        Invoke(nameof(ResetState), _attackData.AttackDuration);
    }

    private IEnumerator LungeForward()
    {
        // Wait a little bit so the lunge ends exactly when the player attacks
        yield return new WaitForSeconds(_attackData.ChargeDuration - _attackData.LungeDuration);

        _stateManager.SetAttacking();

        Collider[] enemies = CastHitbox();

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

    private void StartAttack()
    {
        Collider[] hits = CastHitbox();

        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent<Health>(out var health)) return;

            SuccesfulAttack?.Invoke(hit, _attackData.AttackDamage);
            health.TakeDamage(_attackData.AttackDamage);
        }
    }

    private void ResetAttack()
    {
        _stateManager.SetRecharging();

        CanAttack = true;
    }

    private void ResetState()
    {
        if (_stateManager.IsRecharging())
        {
            _stateManager.SetIdle();
        }
    }

    private Collider[] CastHitbox()
    {
        return Physics.OverlapBox(_attackPoint.position, _attackData.AttackSize * 0.5f, _attackPoint.rotation, _attackData.EnemyLayer);
    }
}
