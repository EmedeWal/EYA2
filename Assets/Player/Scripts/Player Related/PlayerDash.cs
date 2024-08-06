using System;
using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private AudioSource _audioSource;
    private CharacterController _controller;
    private PlayerStateManager _stateManager;
    private PlayerInputManager _inputManager;
    private PlayerDataManager _dataManager;
    private Animator _animator;
    private Health _health;

    [Header("VARIABLES")]
    [SerializeField] private float _dashSpeed = 45f;
    [SerializeField] private float _dashCD = 6f;
    [SerializeField] private float _dashDuration = 0.3f;

    private float _currentCD;
    private bool _canDash = true;

    public delegate void Delegate_DashCooldown(float cooldown);
    public event Delegate_DashCooldown DashCooldownStart;
    public event Delegate_DashCooldown DashCooldownUpdate;
    public event Action DashEnd;

    private void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _inputManager = GetComponent<PlayerInputManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _inputManager.DashInput_Performed += PlayerDash_DashInput_Performed;
    }

    private void OnDisable()
    {
        _inputManager.DashInput_Performed -= PlayerDash_DashInput_Performed;
    }

    private void PlayerDash_DashInput_Performed()
    {
        if (_canDash)
        {
            StartCoroutine(DashCoroutine());
            StartCoroutine(DashCooldownCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        _animator.SetFloat("Speed", 1f);
        _trailRenderer.enabled = true;
        _health.SetInvincible(true);
        _stateManager.SetDashing();
        _audioSource.Play();
        _canDash = false;

        float startTime = Time.time;

        while (Time.time < startTime + _dashDuration)
        {
            _controller.Move(_dashSpeed * Time.deltaTime * transform.forward);
            yield return null;
        }

        _trailRenderer.enabled = false;
        _health.SetInvincible(false);
        _stateManager.SetIdle();
        OnDashEnd();
    }

    private IEnumerator DashCooldownCoroutine()
    {
        OnDashCooldownStart(_dashCD);
        _currentCD = _dashCD;

        while (_currentCD > 0f)
        {
            _currentCD -= Time.deltaTime * _dataManager.GetDashModifier();
            OnDashCooldownUpdate(_currentCD);
            yield return null;
        }

        _canDash = true;
    }

    private void OnDashCooldownStart(float time)
    {
        DashCooldownStart?.Invoke(time);
    }

    private void OnDashCooldownUpdate(float time)
    {
        DashCooldownUpdate?.Invoke(time);
    }

    private void OnDashEnd()
    {
        DashEnd?.Invoke();
    }
}
