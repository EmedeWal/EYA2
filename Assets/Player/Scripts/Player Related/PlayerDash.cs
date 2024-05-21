using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private AudioSource _audioSource;
    private PlayerStateManager _stateManager;
    private PlayerInputManager _inputManager;
    private PlayerDataManager _dataManager;
    private CharacterController _controller;
    private PlayerHealth _health;
    private Animator _animator;

    [Header("VARIABLES")]
    [SerializeField] private float _dashSpeed = 45f;
    [SerializeField] private float _dashCD = 6f;
    [SerializeField] private float _dashDuration = 0.3f;

    private float _currentCD;
    private bool _canDash = true;

    // Events
    public delegate void Delegate_CooldownStart(float dashCooldown);
    public static event Delegate_CooldownStart CooldownStart;

    public delegate void Delegate_CooldownCountdown(float currentCooldown);
    public static event Delegate_CooldownCountdown CooldownCountdown;

    public delegate void Delegate_DashEnd();
    public static event Delegate_DashEnd DashEnd;

    private void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _inputManager = GetComponent<PlayerInputManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _controller = GetComponent<CharacterController>();
        _health = GetComponent<PlayerHealth>();
        _animator = GetComponentInChildren<Animator>();
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
        if (_canDash) StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        StartCoroutine(DashCooldownCountdown());

        _animator.SetFloat("Speed", 1f);

        _health.SetInvincible(true);

        _stateManager.SetDashing();

        _audioSource.Play();

        _canDash = false;

        _trailRenderer.enabled = true;

        float startTime = Time.time;

        while (Time.time < startTime + _dashDuration)
        {
            _controller.Move(_dashSpeed * Time.deltaTime * transform.forward);
            yield return null;
        }

        _trailRenderer.enabled = false;

        _health.SetInvincible(false);

        _stateManager.SetIdle();

        DashEnd?.Invoke();
    }

    private IEnumerator DashCooldownCountdown()
    {
        CooldownStart?.Invoke(_dashCD);

        _currentCD = _dashCD;

        while (_currentCD > 0f)
        {
            _currentCD -= Time.deltaTime * _dataManager.GetDashModifier();

            CooldownCountdown?.Invoke(_currentCD);

            yield return null;
        }
        
        ResetDash();
    }

    private void ResetDash()
    {
        _canDash = true;
    }
}
