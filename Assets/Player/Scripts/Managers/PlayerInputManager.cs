using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerStateManager _stateManager;

    [Header("VARIABLES")]
    [SerializeField] private float _retryDuration = 0.2f;
    private Action _lastAction;
    private float _lastInputTime;

    [Header("INPUT ACTION ASSET")]
    [SerializeField] private InputActionAsset _inputActionAsset;

    private InputAction _directionAction;
    private InputAction _dashAction;
    private InputAction _lightAttackAction;
    private InputAction _heavyAttackAction;
    private InputAction _swapStanceAction;
    private InputAction _ultimateAction;
    private InputAction _consumePotionAction;
    private InputAction _interactionAction;
    private InputAction _pauseAction;
    private InputAction _swapMenuAction;
    private InputAction _skipAction;

    public event Action SkipInput_Performed;
    public event Action DashInput_Performed;
    public event Action PauseInput_Performed;
    public event Action UltimateInput_Performed;
    public event Action<Vector2> DirectionInput_Value;
    public event Action InteractionInput_Performed;
    public event Action<float> SwapMenuInput_Performed;
    public event Action LightAttackInput_Performed;
    public event Action HeavyAttackInput_Performed;
    public event Action SwapStanceInput_Performed;
    public event Action<float> ConsumePotionInput_Performed;

    private void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _skipAction = _inputActionAsset.FindAction("Skip");
        _dashAction = _inputActionAsset.FindAction("Dash");
        _pauseAction = _inputActionAsset.FindAction("Pause");
        _ultimateAction = _inputActionAsset.FindAction("Ultimate");
        _directionAction = _inputActionAsset.FindAction("Direction");
        _interactionAction = _inputActionAsset.FindAction("Interaction");
        _swapMenuAction = _inputActionAsset.FindAction("Swap Menu");
        _lightAttackAction = _inputActionAsset.FindAction("Light Attack");
        _heavyAttackAction = _inputActionAsset.FindAction("Heavy Attack");
        _swapStanceAction = _inputActionAsset.FindAction("Swap Stance");
        _consumePotionAction = _inputActionAsset.FindAction("Consume Potion");
    }

    private void OnEnable()
    {
        _skipAction.Enable();
        _dashAction.Enable();
        _pauseAction.Enable();
        _ultimateAction.Enable();
        _directionAction.Enable();
        _interactionAction.Enable();
        _swapMenuAction.Enable();
        _lightAttackAction.Enable();
        _heavyAttackAction.Enable();
        _swapStanceAction.Enable();
        _consumePotionAction.Enable();

        _skipAction.performed += OnSkipInput_Performed;
        _dashAction.performed += OnDashInput_Performed;
        _pauseAction.performed += OnPauseInput_Performed;
        _ultimateAction.performed += OnUltimateInput_Performed;
        _directionAction.performed += OnDirectionInput;
        _directionAction.canceled += OnDirectionInput;
        _interactionAction.performed += OnInteractionInput_Performed;
        _swapMenuAction.performed += OnSwapMenuInput_Performed;
        _lightAttackAction.performed += OnLightAttackInput_Performed;
        _heavyAttackAction.performed += OnHeavyAttackInput_Performed;
        _swapStanceAction.performed += OnSwapStanceInput_Performed;
        _consumePotionAction.performed += OnConsumePotionInput_Performed;
    }

    private void OnDisable()
    {
        _skipAction.Disable();
        _dashAction.Disable();
        _pauseAction.Disable();
        _ultimateAction.Disable();
        _directionAction.Disable();
        _interactionAction.Disable();
        _swapMenuAction.Disable();
        _lightAttackAction.Disable();
        _heavyAttackAction.Disable();
        _swapStanceAction.Disable();
        _consumePotionAction.Disable();

        _skipAction.performed -= OnSkipInput_Performed;
        _dashAction.performed -= OnDashInput_Performed;
        _pauseAction.performed -= OnPauseInput_Performed;
        _ultimateAction.performed -= OnUltimateInput_Performed;
        _directionAction.performed -= OnDirectionInput;
        _directionAction.canceled -= OnDirectionInput;
        _interactionAction.performed -= OnInteractionInput_Performed;
        _swapMenuAction.performed -= OnSwapMenuInput_Performed;
        _lightAttackAction.performed -= OnLightAttackInput_Performed;
        _heavyAttackAction.performed -= OnHeavyAttackInput_Performed;
        _swapStanceAction.performed -= OnSwapStanceInput_Performed;
        _consumePotionAction.performed -= OnConsumePotionInput_Performed;
    }

    private void Update()
    {
        if (_lastAction != null && (Time.time - _lastInputTime <= _retryDuration))
        {
            _lastAction.Invoke();
        }
    }
    private void HandleInput(Action action, Func<bool> canPerformAction)
    {
        if (!canPerformAction()) return;

        _lastAction = () => { if (canPerformAction()) action.Invoke(); };
        _lastInputTime = Time.time;
        _lastAction.Invoke();
    }

    private void OnSkipInput_Performed(InputAction.CallbackContext context)
    {
        SkipInput_Performed?.Invoke();
    }

    private void OnDashInput_Performed(InputAction.CallbackContext context)
    {
        HandleInput(() => DashInput_Performed?.Invoke(), _stateManager.CanDash);
    }

    private void OnPauseInput_Performed(InputAction.CallbackContext context)
    {
        PauseInput_Performed?.Invoke();
    }

    private void OnUltimateInput_Performed(InputAction.CallbackContext context)
    {
        UltimateInput_Performed?.Invoke();
    }

    private void OnDirectionInput(InputAction.CallbackContext context)
    {
        DirectionInput_Value?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnInteractionInput_Performed(InputAction.CallbackContext context)
    {
        InteractionInput_Performed?.Invoke();
    }

    private void OnSwapMenuInput_Performed(InputAction.CallbackContext context)
    {
        SwapMenuInput_Performed?.Invoke(context.ReadValue<float>());
    }

    private void OnLightAttackInput_Performed(InputAction.CallbackContext context)
    {
        HandleInput(() => LightAttackInput_Performed?.Invoke(), _stateManager.CanAttack);
    }

    private void OnHeavyAttackInput_Performed(InputAction.CallbackContext context)
    {
        HandleInput(() => HeavyAttackInput_Performed?.Invoke(), _stateManager.CanAttack);
    }

    private void OnSwapStanceInput_Performed(InputAction.CallbackContext context)
    {
        SwapStanceInput_Performed?.Invoke();
    }

    private void OnConsumePotionInput_Performed(InputAction.CallbackContext context)
    {
        ConsumePotionInput_Performed?.Invoke(context.ReadValue<float>());
    }
}

