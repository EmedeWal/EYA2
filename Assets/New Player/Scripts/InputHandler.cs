using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace Player
{
    public class InputHandler : SingletonBase
    {
        #region Singleton
        public static InputHandler Instance { get; private set; }

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

        public Vector2 LeftStickInput { get; private set; }
        public Vector2 RightStickInput { get; private set; }

        public event Action LockOnInputPerformed;
        public event Action UltimateInputPerformed;

        public event Action SwapStanceInputPerformed;
        public event Action LightAttackInputPerformed;
        public event Action HeavyAttackInputPerformed;
        public event Action BlockInputPerformed;
        public event Action SpecialInputPerformed;

        public event Action BackInputPerformed;
        public event Action ClickInputPerformed;
        public event Action PauseInputPerformed;
        public event Action<int> SwapHeaderInputPerformed;
        public event Action<int> MoveSliderInputPerformed;
        public event Action<int> SwapSectionInputPerformed;

        InputActions _inputActions;

        //[Header("SETTINGS")]
        //[SerializeField] private float _comboInputTimer = 0.1f;
        private bool _shoulderPressed = false;
        private bool _stickPressed = false;


        public void Init()
        {
            _inputActions ??= new InputActions();

            _inputActions.Movement.LeftStick.performed += indexer => LeftStickInput = indexer.ReadValue<Vector2>();
            _inputActions.Movement.LeftStick.canceled += indexer => LeftStickInput = indexer.ReadValue<Vector2>();
            _inputActions.Movement.RightStick.performed += indexer => RightStickInput = indexer.ReadValue<Vector2>();
            _inputActions.Movement.RightStick.canceled += indexer => RightStickInput = indexer.ReadValue<Vector2>();

            _inputActions.Actions.DPadUpDown.performed += OnSwapSectionInputPerformed;
            _inputActions.Actions.DPadLeftRight.performed += OnMoveSliderInputPerformed;
            _inputActions.Actions.Options.performed += OnPauseInputPerformed;
            _inputActions.Actions.Shoulders.performed += OnSwapHeaderInputPerformed;
            _inputActions.Actions.ButtonEast.performed += OnBackInputPerformed;
            _inputActions.Actions.ButtonSouth.performed += OnClickInputPerformed;

            ListenToCombatActions(true);

            _inputActions.Enable();
        }

        public void Cleanup()
        {
            _inputActions.Movement.LeftStick.performed -= indexer => LeftStickInput = indexer.ReadValue<Vector2>();
            _inputActions.Movement.LeftStick.canceled -= indexer => LeftStickInput = indexer.ReadValue<Vector2>();
            _inputActions.Movement.RightStick.performed -= indexer => RightStickInput = indexer.ReadValue<Vector2>();
            _inputActions.Movement.RightStick.canceled -= indexer => RightStickInput = indexer.ReadValue<Vector2>();

            _inputActions.Actions.DPadUpDown.performed -= OnSwapSectionInputPerformed;
            _inputActions.Actions.DPadLeftRight.performed -= OnMoveSliderInputPerformed;
            _inputActions.Actions.Options.performed -= OnPauseInputPerformed;
            _inputActions.Actions.Shoulders.performed -= OnSwapHeaderInputPerformed;
            _inputActions.Actions.ButtonEast.performed -= OnBackInputPerformed;
            _inputActions.Actions.ButtonSouth.performed -= OnClickInputPerformed;

            ListenToCombatActions(false);

            _inputActions.Disable();
        }

        public void ListenToCombatActions(bool subscribe)
        {
            if (subscribe)
            {
                _inputActions.Actions.L3Press.performed += HandleL3PressPerformed;
                _inputActions.Actions.R3Press.performed += HandleR3PressPerformed;
                _inputActions.Actions.LeftTrigger.performed += OnUltimateInputPerformed;
                _inputActions.Actions.LeftShoulder.performed += OnSwapStanceInputPerformed;
                _inputActions.Actions.RightShoulder.performed += OnLightAttackInputPerformed;
                _inputActions.Actions.RightTrigger.performed += OnHeavyAttackInputPerformed;
            }
            else
            {
                _inputActions.Actions.L3Press.performed -= HandleL3PressPerformed;
                _inputActions.Actions.R3Press.performed -= HandleR3PressPerformed;
                _inputActions.Actions.LeftTrigger.performed -= OnUltimateInputPerformed;
                _inputActions.Actions.LeftShoulder.performed -= OnSwapStanceInputPerformed;
                _inputActions.Actions.RightShoulder.performed -= OnLightAttackInputPerformed;
                _inputActions.Actions.RightTrigger.performed -= OnHeavyAttackInputPerformed;
            }
        }

        #region Combat Actions

        private void HandleL3PressPerformed(InputAction.CallbackContext context)
        {
            if (_stickPressed)
            {
                OnUltimateInputPerformed(context);
            }
            else
            {
                //_stickPressed = true;
                //Invoke(nameof(ResetStickPressed), _comboInputTimer);
            }
        }

        private void HandleR3PressPerformed(InputAction.CallbackContext context)
        {
            if (_stickPressed)
            {
                OnUltimateInputPerformed(context);
            }
            else
            {
                //_stickPressed = true;
                //Invoke(nameof(ResetStickPressed), _comboInputTimer);

                OnLockOnInputPerformed(context);
            }
        }

        private void HandleLeftTriggerPerformed(InputAction.CallbackContext context)
        {
            if (_shoulderPressed)
            {
                OnSpecialInputPerformed(context);
            }
            else
            {
                //_shoulderPressed = true;
                //Invoke(nameof(ResetShoulderPressed), _comboInputTimer);

                OnBlockInputPerformed(context);
            }
        }

        private void HandleRightTriggerPerformed(InputAction.CallbackContext context)
        {
            if (_shoulderPressed)
            {
                OnSpecialInputPerformed(context);
            }
            else
            {
                //_shoulderPressed = true;
                //Invoke(nameof(ResetShoulderPressed), _comboInputTimer);

                OnLightAttackInputPerformed(context);
            }
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
            LightAttackInputPerformed?.Invoke();
        }

        private void OnHeavyAttackInputPerformed(InputAction.CallbackContext context)
        {
            HeavyAttackInputPerformed?.Invoke();
        }

        private void OnBlockInputPerformed(InputAction.CallbackContext context)
        {
            BlockInputPerformed?.Invoke();
        }

        private void OnSpecialInputPerformed(InputAction.CallbackContext context)
        {
            SpecialInputPerformed?.Invoke();
        }
        #endregion

        private void OnBackInputPerformed(InputAction.CallbackContext context)
        {
            BackInputPerformed?.Invoke();
        }

        private void OnClickInputPerformed(InputAction.CallbackContext context)
        {
            ClickInputPerformed?.Invoke();
        }

        private void OnPauseInputPerformed(InputAction.CallbackContext context)
        {
            PauseInputPerformed?.Invoke();
        }

        private void OnSwapHeaderInputPerformed(InputAction.CallbackContext context)
        {
            SwapHeaderInputPerformed?.Invoke(Mathf.FloorToInt(context.ReadValue<float>()));
        }

        private void OnMoveSliderInputPerformed(InputAction.CallbackContext context)
        {
            MoveSliderInputPerformed?.Invoke(Mathf.FloorToInt(context.ReadValue<float>()));
        }

        private void OnSwapSectionInputPerformed(InputAction.CallbackContext context)
        {
            SwapSectionInputPerformed?.Invoke(Mathf.FloorToInt(context.ReadValue<float>()));
        }

        private void ResetShoulderPressed()
        {
            _shoulderPressed = false;
        }

        private void ResetStickPressed()
        {
            _stickPressed = false;
        }
    }
}