using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private PlayerAnimatorManager _animatorManager;
    private PlayerStanceManager _stanceManager;
    private PlayerDataManager _dataManager;
    private PlayerLockOn _lockOn;
    private PlayerLocomotion _locomotion;
    private PlayerAttackHandler _attackHandler;
    private Health _health;
    private Souls _souls;
    private Mana _mana;
    private MovementTracking _movementTracking;
    private CameraController _cameraManager;
    
    private float _delta;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);

        _inputHandler = GetComponent<PlayerInputHandler>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _stanceManager = GetComponent<PlayerStanceManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _lockOn = GetComponent<PlayerLockOn>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _attackHandler = GetComponent<PlayerAttackHandler>();
        _health = GetComponent<Health>();
        _souls = GetComponent<Souls>();
        _mana = GetComponent<Mana>();
        _movementTracking = GetComponent<MovementTracking>();
        _cameraManager = CameraController.Instance;

        _inputHandler.Init();
        _animatorManager.Init();
        _stanceManager.Init();
        _dataManager.Init();
        _lockOn.Init();
        _locomotion.Init();
        _attackHandler.Init();
        _health.Init();
        _souls.Init();
        _mana.Init();   
        _movementTracking.Init();
        _cameraManager.Init(transform);

        _health.AddConstantValue(1, 1);
    }

    private void Update()
    {
        _delta = Time.deltaTime;

        _inputHandler.Tick();
        _locomotion.Tick();
        _attackHandler.Tick(_delta);
        _movementTracking.Tick(_delta);
    }

    private void FixedUpdate()
    {
        _delta = Time.fixedDeltaTime;

        Vector3 xDirection = _cameraManager._CameraTransform.right;
        Vector3 yDirection = _cameraManager._CameraTransform.forward;
        float leftStickX = _inputHandler._LeftStickX;
        float leftStickY = _inputHandler._LeftStickY;
        float rightStickX = _inputHandler._RightStickX;
        float rightStickY = _inputHandler._RightStickY;

        Transform lockOnTargetTransform = _dataManager.LockOnStruct.LockOnTargetTransform;
        bool lockedOn = _dataManager.LockOnStruct.LockedOn;

        _locomotion.FixedTick(_delta, xDirection, yDirection, leftStickX, leftStickY, lockOnTargetTransform, lockedOn);
        _lockOn.FixedTick();
        _cameraManager.FixedTick(_delta, rightStickX, rightStickY, lockOnTargetTransform, lockedOn);
    }
}
