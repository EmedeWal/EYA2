using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    private float _delta;

    [Header("PLAYER UI REFERENCES")]
    [SerializeField] private StanceUI _stanceUI;

    private PlayerInputHandler _inputHandler;
    private PlayerAnimatorManager _animatorManager;
    private PlayerStanceManager _stanceManager;
    private PlayerStatManager _statManager;
    private PlayerLock _lock;
    private PlayerLocomotion _locomotion;
    private PlayerAttackHandler _attackHandler;
    private Health _health;
    private Souls _souls;
    private MovementTracking _movementTracking;
    private FootstepHandler _footstepHandler;
    private CameraController _cameraController;
    private TimeSystem _timeSystem;

    private Transform _target = null;

    public static event Action<AnimatorManager> PlayerDeath;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);

        _inputHandler = GetComponent<PlayerInputHandler>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _stanceManager = GetComponent<PlayerStanceManager>();
        _statManager = GetComponent<PlayerStatManager>();
        _lock = GetComponent<PlayerLock>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _attackHandler = GetComponent<PlayerAttackHandler>();
        _health = GetComponent<Health>();
        _souls = GetComponent<Souls>();
        _movementTracking = GetComponent<MovementTracking>();
        _footstepHandler = GetComponent<FootstepHandler>();
        _cameraController = CameraController.Instance;
        _timeSystem = TimeSystem.Instance;  

        _stanceUI.Init();

        _inputHandler.Init();
        _animatorManager.Init();
        _stanceManager.Init();
        _statManager.Init();
        _lock.Init();
        _locomotion.Init();
        _attackHandler.Init(LayerMask.GetMask("DamageCollider"));
        _souls.Init();
        _movementTracking.Init();
        _footstepHandler.Init();
        _cameraController.Init(transform);

        _lock.Locked += PlayerManager_LockedOn;
        _health.ValueExhausted += PlayerManager_ValueExhausted;
    }

    public void Tick(float delta)
    {
        _delta = delta;

        Vector3 xDirection = _cameraController._CameraTransform.right;
        Vector3 yDirection = _cameraController._CameraTransform.forward;
        float leftStickX = _inputHandler._LeftStickX;
        float leftStickY = _inputHandler._LeftStickY;
        float rightStickX = _inputHandler._RightStickX;
        float rightStickY = _inputHandler._RightStickY;

        _inputHandler.Tick();

        if (_timeSystem.CurrentTimeScale == 0) return;

        _stanceManager.Tick(_delta);
        _locomotion.Tick(_delta, xDirection, yDirection, leftStickX, leftStickY, _target);
        _attackHandler.Tick(_delta);
        _movementTracking.Tick(_delta);
        _cameraController.Tick(_delta, rightStickX, rightStickY, _target);
    }

    public void LateTick(float delta)
    {
        _delta = delta;

        _statManager.LateTick(_delta);
    }

    public void Cleanup()
    {
        _lock.Locked -= PlayerManager_LockedOn;
        _health.ValueExhausted -= PlayerManager_ValueExhausted;

        _stanceUI.Cleanup();

        _stanceManager.Cleanup();
        _statManager.Cleanup();
        _lock.Cleanup();
        _locomotion.Cleanup();
        _attackHandler.Cleanup();
        _souls.Cleanup();
    }

    private void PlayerManager_LockedOn(Transform target)
    {
        _target = target;
    }

    private void PlayerManager_ValueExhausted(GameObject playerObject)
    {
        _animatorManager.ForceCrossFade(_delta, "Death");

        Cleanup();

        Destroy(_inputHandler);
        Destroy(_stanceManager);
        Destroy(_statManager);
        Destroy(_lock);
        Destroy(_locomotion);
        Destroy(_attackHandler);
        Destroy(_health);
        Destroy(_cameraController);

        OnPlayerDeath();

        Destroy(this);
    }

    private void OnPlayerDeath()
    {
        PlayerDeath?.Invoke(_animatorManager);
    }
}