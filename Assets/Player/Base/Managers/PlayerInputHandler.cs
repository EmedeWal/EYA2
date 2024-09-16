using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerInputHandler : SingletonBase
{
    [Header("VARIABLES")]
    [SerializeField] private float _retryDuration = 0.2f;
    private Action _lastAction;
    private float _lastInputTime;

    #region Singleton
    public static PlayerInputHandler Instance { get; private set; }

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Left Stick
    private Vector2 _leftStickValue;
    public float _LeftStickX { get; private set; }
    public float _LeftStickY { get; private set; }
    #endregion

    #region Right Stick
    private Vector2 _rightStickValue;
    public float _RightStickX {  get; private set; }
    public float _RightStickY { get; private set; }
    #endregion

    public event Action LockOnInputPerformed;
    public event Action UltimateInputPerformed;
    public event Action SwapStanceInputPerformed;
    public event Action LightAttackInputPerformed;
    public event Action HeavyAttackInputPerformed;

    #region Input Setup
    InputActions _inputActions;

    private void OnEnable()
    {
        _inputActions ??= new InputActions();

        _inputActions.Movement.LeftStick.performed += indexer => _leftStickValue = indexer.ReadValue<Vector2>();
        _inputActions.Movement.LeftStick.canceled += indexer => _leftStickValue = indexer.ReadValue<Vector2>();
        _inputActions.Movement.RightStick.performed += indexer => _rightStickValue = indexer.ReadValue<Vector2>();
        _inputActions.Movement.RightStick.canceled += indexer => _rightStickValue = indexer.ReadValue<Vector2>();

        _inputActions.Actions.R3Press.performed += OnLockOnInputPerformed;
        _inputActions.Actions.LeftTrigger.performed += OnUltimateInputPerformed;
        _inputActions.Actions.LeftShoulder.performed += OnSwapStanceInputPerformed;
        _inputActions.Actions.RightShoulder.performed += OnLightAttackInputPerformed;
        _inputActions.Actions.RightTrigger.performed += OnHeavyAttackInputPerformed;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();   
    }
    #endregion

    public void Tick()
    {
        if (_lastAction != null && (Time.time - _lastInputTime <= _retryDuration))
        {
            _lastAction.Invoke();
        }
    }

    public void FixedTick()
    {
        _LeftStickX = _leftStickValue.x;
        _LeftStickY = _leftStickValue.y;

        _RightStickX = _rightStickValue.x;
        _RightStickY = _rightStickValue.y;
    }

    private void HandleInput(Action action)
    {
        _lastAction = () => action.Invoke();
        _lastInputTime = Time.time;
        _lastAction.Invoke();
    }

    private void OnLockOnInputPerformed(InputAction.CallbackContext context)
    {
        LockOnInputPerformed?.Invoke();
    }

    private void OnUltimateInputPerformed(InputAction.CallbackContext context)
    {
        UltimateInputPerformed?.Invoke();
    }

    private void OnSwapStanceInputPerformed(InputAction.CallbackContext context)
    {
        SwapStanceInputPerformed?.Invoke();
    }

    private void OnLightAttackInputPerformed(InputAction.CallbackContext context)
    {
        HandleInput(() => LightAttackInputPerformed?.Invoke());
        LightAttackInputPerformed?.Invoke();
    }

    private void OnHeavyAttackInputPerformed(InputAction.CallbackContext context)
    {
        HandleInput(() => HeavyAttackInputPerformed?.Invoke());
        HeavyAttackInputPerformed?.Invoke();
    }
}

