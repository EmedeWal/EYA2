//using UnityEngine.InputSystem;
//using UnityEngine;
//using System;

//public class PlayerInputHandler : SingletonBase
//{
//    [Header("VARIABLES")]
//    [SerializeField] private float _retryDuration = 0.2f;
//    private Action _lastAction;
//    private float _lastInputTime;

//    #region Singleton
//    public static PlayerInputHandler Instance { get; private set; }

//    public override void SingletonSetup()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//    #endregion

//    #region Left Stick
//    private Vector2 _leftStickValue;
//    public float LeftStickX { get; private set; }
//    public float LeftStickY { get; private set; }
//    #endregion

//    #region Right Stick
//    private Vector2 _rightStickValue;
//    public float RightStickX { get; private set; }
//    public float RightStickY { get; private set; }
//    #endregion

//    public event Action LockOnInputPerformed;
//    public event Action UltimateInputPerformed;
//    public event Action SwapStanceInputPerformed;
//    public event Action LightAttackInputPerformed;
//    public event Action HeavyAttackInputPerformed;

//    public event Action BackInputPerformed;
//    public event Action ClickInputPerformed;
//    public event Action PauseInputPerformed;
//    public event Action<int> SwapHeaderInputPerformed;
//    public event Action<int> MoveSliderInputPerformed;
//    public event Action<int> SwapSectionInputPerformed;

//    InputActions _inputActions;

//    public void Init()
//    {
//        _inputActions ??= new InputActions();

//        _inputActions.Movement.LeftStick.performed += indexer => _leftStickValue = indexer.ReadValue<Vector2>();
//        _inputActions.Movement.LeftStick.canceled += indexer => _leftStickValue = indexer.ReadValue<Vector2>();
//        _inputActions.Movement.RightStick.performed += indexer => _rightStickValue = indexer.ReadValue<Vector2>();
//        _inputActions.Movement.RightStick.canceled += indexer => _rightStickValue = indexer.ReadValue<Vector2>();


//        _inputActions.Actions.ButtonEast.performed += OnBackInputPerformed;
//        _inputActions.Actions.ButtonSouth.performed += OnClickInputPerformed;
//        _inputActions.Actions.Options.performed += OnPauseInputPerformed;
//        _inputActions.Actions.Shoulders.performed += OnSwapHeaderInputPerformed;
//        _inputActions.Actions.DPadLeftRight.performed += OnMoveSliderInputPerformed;
//        _inputActions.Actions.DPadUpDown.performed += OnSwapSectionInputPerformed;

//        ListenToCombatActions(true);

//        _inputActions.Enable();
//    }

//    public void Tick()
//    {
//        if (_lastAction != null && (Time.time - _lastInputTime <= _retryDuration))
//        {
//            _lastAction.Invoke();
//        }

//        CalculateInput();
//    }

//    public void FixedTick()
//    {
//        CalculateInput();
//    }

//    public void Cleanup()
//    {
//        _inputActions.Movement.LeftStick.performed -= indexer => _leftStickValue = indexer.ReadValue<Vector2>();
//        _inputActions.Movement.LeftStick.canceled -= indexer => _leftStickValue = indexer.ReadValue<Vector2>();
//        _inputActions.Movement.RightStick.performed -= indexer => _rightStickValue = indexer.ReadValue<Vector2>();
//        _inputActions.Movement.RightStick.canceled -= indexer => _rightStickValue = indexer.ReadValue<Vector2>();

//        _inputActions.Actions.ButtonEast.performed -= OnBackInputPerformed;
//        _inputActions.Actions.ButtonSouth.performed -= OnClickInputPerformed;
//        _inputActions.Actions.Options.performed -= OnPauseInputPerformed;
//        _inputActions.Actions.Shoulders.performed -= OnSwapHeaderInputPerformed;
//        _inputActions.Actions.DPadLeftRight.performed -= OnMoveSliderInputPerformed;
//        _inputActions.Actions.DPadUpDown.performed -= OnSwapSectionInputPerformed;

//        ListenToCombatActions(false);

//        _inputActions.Disable();
//    }

//    public void ListenToCombatActions(bool subscribe)
//    {
//        if (subscribe)
//        {
//            _inputActions.Actions.R3Press.performed += OnLockOnInputPerformed;
//            _inputActions.Actions.LeftTrigger.performed += OnUltimateInputPerformed;
//            _inputActions.Actions.LeftShoulder.performed += OnSwapStanceInputPerformed;
//            _inputActions.Actions.RightShoulder.performed += OnLightAttackInputPerformed;
//            _inputActions.Actions.RightTrigger.performed += OnHeavyAttackInputPerformed;
//        }
//        else
//        {
//            _inputActions.Actions.R3Press.performed -= OnLockOnInputPerformed;
//            _inputActions.Actions.LeftTrigger.performed -= OnUltimateInputPerformed;
//            _inputActions.Actions.LeftShoulder.performed -= OnSwapStanceInputPerformed;
//            _inputActions.Actions.RightShoulder.performed -= OnLightAttackInputPerformed;
//            _inputActions.Actions.RightTrigger.performed -= OnHeavyAttackInputPerformed;
//        }
//    }

//    private void HandleInput(Action action)
//    {
//        _lastAction = () => action.Invoke();
//        _lastInputTime = Time.time;
//        _lastAction.Invoke();
//    }

//    private void CalculateInput()
//    {
//        LeftStickX = _leftStickValue.x;
//        LeftStickY = _leftStickValue.y;

//        RightStickX = _rightStickValue.x;
//        RightStickY = _rightStickValue.y;
//    }

//    #region Combat Actions
//    private void OnLockOnInputPerformed(InputAction.CallbackContext context)
//    {
//        LockOnInputPerformed?.Invoke();
//    }

//    private void OnUltimateInputPerformed(InputAction.CallbackContext context)
//    {
//        UltimateInputPerformed?.Invoke();
//    }

//    private void OnSwapStanceInputPerformed(InputAction.CallbackContext context)
//    {
//        SwapStanceInputPerformed?.Invoke();
//    }

//    private void OnLightAttackInputPerformed(InputAction.CallbackContext context)
//    {
//        HandleInput(() => LightAttackInputPerformed?.Invoke());
//        LightAttackInputPerformed?.Invoke();
//    }

//    private void OnHeavyAttackInputPerformed(InputAction.CallbackContext context)
//    {
//        HandleInput(() => HeavyAttackInputPerformed?.Invoke());
//        HeavyAttackInputPerformed?.Invoke();
//    }
//    #endregion

//    private void OnBackInputPerformed(InputAction.CallbackContext context)
//    {
//        BackInputPerformed?.Invoke();   
//    }

//    private void OnClickInputPerformed(InputAction.CallbackContext context)
//    {
//        ClickInputPerformed?.Invoke();
//    }

//    private void OnPauseInputPerformed(InputAction.CallbackContext context)
//    {
//        PauseInputPerformed?.Invoke();
//    }

//    private void OnSwapHeaderInputPerformed(InputAction.CallbackContext context)
//    {
//        SwapHeaderInputPerformed?.Invoke(Mathf.FloorToInt(context.ReadValue<float>()));
//    }

//    private void OnMoveSliderInputPerformed(InputAction.CallbackContext context)
//    {
//        MoveSliderInputPerformed?.Invoke(Mathf.FloorToInt(context.ReadValue<float>()));
//    }

//    private void OnSwapSectionInputPerformed(InputAction.CallbackContext context)
//    {
//        SwapSectionInputPerformed?.Invoke(Mathf.FloorToInt(context.ReadValue<float>()));
//    }
//}

