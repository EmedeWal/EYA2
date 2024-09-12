using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _retryDuration = 0.2f;
    private Action _lastAction;
    private float _lastInputTime;

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

    public event Action StanceWheelInput;
    public event Action LockOnInputPerformed;
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

        _inputActions.Actions.LeftShoulder.performed += OnStanceWheelInput;
        _inputActions.Actions.LeftShoulder.canceled += OnStanceWheelInput;
        _inputActions.Actions.R3Press.performed += OnLockOnInputPerformed;
        _inputActions.Actions.RightShoulder.performed += OnLightAttackInputPerformed;
        _inputActions.Actions.RightTrigger.performed += OnHeavyAttackInputPerformed;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();   
    }
    #endregion

    public void OnUpdate()
    {
        if (_lastAction != null && (Time.time - _lastInputTime <= _retryDuration))
        {
            _lastAction.Invoke();
        }
    }

    public void OnFixedUpdate()
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

    private void OnStanceWheelInput(InputAction.CallbackContext context)
    {
        StanceWheelInput?.Invoke();
    }

    private void OnLockOnInputPerformed(InputAction.CallbackContext context)
    {
        LockOnInputPerformed?.Invoke();
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

