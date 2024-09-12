using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private PlayerAnimatorManager _animatorManager;
    private PlayerDataManager _dataManager;
    private PlayerLocomotion _locomotion;
    private PlayerAttackHandler _attackHandler;
    private CameraManager _cameraManager;
    
    private float _delta;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _inputHandler = GetComponent<PlayerInputHandler>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _dataManager = GetComponent<PlayerDataManager>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _attackHandler = GetComponent<PlayerAttackHandler>();
        _cameraManager = CameraManager.Instance;

        _animatorManager.Initialize();
        _dataManager.Initialize();
        _locomotion.Initialize();
        _cameraManager.Initialize(transform);

        Resource[] resources = GetComponents<Resource>();
        foreach (Resource resource in resources) resource.Initialize();
    }

    private void Update()
    {
        _delta = Time.deltaTime;

        _inputHandler.OnUpdate();
        _locomotion.OnUpdate();
        _attackHandler.OnUpdate(_delta);
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

        Transform lockOnTarget = _dataManager.LockOnData.LockOnTarget;
        bool lockedOn = _dataManager.LockOnData.LockedOn;

        _inputHandler.OnFixedUpdate();
        _locomotion.OnFixedUpdate(_delta, xDirection, yDirection, leftStickX, leftStickY, lockOnTarget, lockedOn);
        _cameraManager.OnFixedUpdate(_delta, rightStickX, rightStickY, lockOnTarget, lockedOn);
    }
}
