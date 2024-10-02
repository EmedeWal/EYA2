using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("PLAYER UI REFERENCES")]
    [SerializeField] private StanceUI _stanceUI;

    private PlayerInputHandler _inputHandler;
    private PlayerStanceManager _stanceManager;
    private PlayerStatManager _statManager;
    private PlayerLockOn _lockOn;
    private PlayerLocomotion _locomotion;
    private PlayerAttackHandler _attackHandler;
    private Health _health;
    private Souls _souls;
    private MovementTracking _movementTracking;
    private CameraController _cameraController;
    private TimeSystem _timeSystem;

    private Transform _target = null;

    public static event Action PlayerDied;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);

        _inputHandler = GetComponent<PlayerInputHandler>();
        _stanceManager = GetComponent<PlayerStanceManager>();
        _statManager = GetComponent<PlayerStatManager>();
        _lockOn = GetComponent<PlayerLockOn>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _attackHandler = GetComponent<PlayerAttackHandler>();
        _health = GetComponent<Health>();
        _souls = GetComponent<Souls>();
        _movementTracking = GetComponent<MovementTracking>();
        _cameraController = CameraController.Instance;
        _timeSystem = TimeSystem.Instance;  

        _stanceUI.Init();

        _inputHandler.Init();
        _stanceManager.Init();
        _statManager.Init();
        _lockOn.Init();
        _locomotion.Init();
        _attackHandler.Init(LayerMask.GetMask("DamageCollider"));
        _souls.Init();
        _movementTracking.Init();
        _cameraController.Init(transform);

        _lockOn.LockedOn += PlayerManager_LockedOn;
        _health.ValueExhausted += PlayerManager_ValueExhausted;
    }

    public void Tick(float delta)
    {
        Vector3 xDirection = _cameraController._CameraTransform.right;
        Vector3 yDirection = _cameraController._CameraTransform.forward;
        float leftStickX = _inputHandler._LeftStickX;
        float leftStickY = _inputHandler._LeftStickY;
        float rightStickX = _inputHandler._RightStickX;
        float rightStickY = _inputHandler._RightStickY;

        _inputHandler.Tick();

        if (_timeSystem.CurrentTimeScale == 0) return;

        _stanceManager.Tick(delta);
        _lockOn.Tick();
        _locomotion.Tick(delta, xDirection, yDirection, leftStickX, leftStickY, _target);
        _attackHandler.Tick(delta);
        _movementTracking.Tick(delta);
        _cameraController.Tick(delta, rightStickX, rightStickY, _target);
    }

    public void LateTick(float delta)
    {
        _statManager.LateTick(delta);
    }

    public void Cleanup()
    {
        _stanceManager.Cleanup();
        _statManager.Cleanup();
        _lockOn.Cleanup();
        _attackHandler.Cleanup();
    }

    private void PlayerManager_LockedOn(Transform target)
    {
        _target = target;
    }

    private void PlayerManager_ValueExhausted()
    {
        _health.ValueExhausted -= PlayerManager_ValueExhausted;

        OnPlayerDied(); 
    }

    private void OnPlayerDied()
    {
        PlayerDied?.Invoke();

        Debug.Log("The player has died. Not implemented");
    }
}
