using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerInputManager : MonoBehaviour
{
    #region Setup
    private PlayerStateManager _stateManager;

    private void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
    }
    #endregion

    #region Retry System

    [Header("VARIABLES")]
    [SerializeField] private float _retryDuration = 0.2f;
    private Action _lastAction;
    private float _lastInputTime;

    private void Update()
    {
        if (_lastAction != null && (Time.time - _lastInputTime <= _retryDuration))
        {
            _lastAction.Invoke();
        }
    }

    #endregion

    public event Action<Vector2> DirectionInput_Value;

    public event Action DashInput_Performed;

    public event Action LightAttackInput_Performed;
    public event Action HeavyAttackInput_Performed;

    public event Action SwapStanceInput_Performed;
    public event Action UltimateInput_Performed;

    public event Action<float> ConsumePotionInput_Performed;

    public event Action InteractionInput_Performed;

    public event Action PauseInput_Performed;
    public event Action<float> SwapMenuInput_Performed;

    public event Action SkipInput_Performed;

    private void HandleInput(Action action, Func<bool> canPerformAction)
    {
        if (!canPerformAction()) return;

        _lastAction = () => { if (canPerformAction()) action.Invoke(); };
        _lastInputTime = Time.time;
        _lastAction.Invoke();
    }

    public void OnDirectionInput(InputAction.CallbackContext context)
    {
        DirectionInput_Value?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleInput(() => DashInput_Performed?.Invoke(), _stateManager.CanDash);
        }
    }

    public void OnLightAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleInput(() => LightAttackInput_Performed?.Invoke(), _stateManager.CanAttack);
        }
    }

    public void OnHeavyAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleInput(() => HeavyAttackInput_Performed?.Invoke(), _stateManager.CanAttack);
        }
    }

    public void OnStanceSwapInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwapStanceInput_Performed?.Invoke();
        }
    }

    public void OnUltimateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UltimateInput_Performed?.Invoke();
        }
    }

    public void OnConsumePotionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ConsumePotionInput_Performed?.Invoke(context.ReadValue<float>());
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractionInput_Performed?.Invoke();
        }
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseInput_Performed?.Invoke();
        }
    }

    public void OnSwapMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwapMenuInput_Performed?.Invoke(context.ReadValue<float>());
        }
    }

    public void OnSkipInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SkipInput_Performed?.Invoke();  
        }
    }
}
