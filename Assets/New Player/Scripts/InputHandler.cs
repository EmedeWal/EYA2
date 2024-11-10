using UnityEngine;
using System;

namespace EmeWillem
{
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

            public Vector2 LeftStickInput { get; private set; } = Vector2.zero;
            public Vector2 RightStickInput { get; private set; } = Vector2.zero;

            public event Action KickInputPerformed;
            public event Action LockInputPerformed;
            public event Action BlockInputPerformed;
            public event Action BlockInputCanceled;
            public event Action SpellInputPerformed;
            public event Action SpellInputCanceled;
            public event Action LightAttackInputPerformed;
            public event Action HeavyAttackInputPerformed;

            public event Action SwapInputPerformed;
            public event Action DodgeInputPerformed;
            public event Action ClimbInputPerformed;
            public event Action InteractInputPerformed;

            public event Action PauseInputPerformed;

            InputActions _inputActions;

            public void Init()
            {
                _inputActions ??= new InputActions();

                _inputActions.Movement.LeftStick.performed += context => LeftStickInput = context.ReadValue<Vector2>();
                _inputActions.Movement.LeftStick.canceled += context => LeftStickInput = context.ReadValue<Vector2>();
                _inputActions.Movement.RightStick.performed += context => RightStickInput = context.ReadValue<Vector2>();
                _inputActions.Movement.RightStick.canceled += context => RightStickInput = context.ReadValue<Vector2>();

                _inputActions.Actions.ButtonWest.performed += context => InteractInputPerformed?.Invoke();
                _inputActions.Actions.Options.performed += context => PauseInputPerformed?.Invoke();

                ListenToCombatActions(true);

                _inputActions.Enable();
            }

            public void Cleanup()
            {
                _inputActions.Movement.LeftStick.performed -= context => LeftStickInput = context.ReadValue<Vector2>();
                _inputActions.Movement.LeftStick.canceled -= context => LeftStickInput = context.ReadValue<Vector2>();
                _inputActions.Movement.RightStick.performed -= context => RightStickInput = context.ReadValue<Vector2>();
                _inputActions.Movement.RightStick.canceled -= context => RightStickInput = context.ReadValue<Vector2>();

                _inputActions.Actions.ButtonWest.performed -= context => InteractInputPerformed?.Invoke();
                _inputActions.Actions.Options.performed -= context => PauseInputPerformed?.Invoke();

                ListenToCombatActions(false);

                _inputActions.Disable();
            }

            public void ListenToCombatActions(bool subscribe)
            {
                if (subscribe)
                {
                    _inputActions.Actions.L3Press.performed += context => KickInputPerformed?.Invoke();
                    _inputActions.Actions.R3Press.performed += context => LockInputPerformed?.Invoke();
                    _inputActions.Actions.LeftShoulder.performed += context => BlockInputPerformed?.Invoke();
                    _inputActions.Actions.LeftShoulder.canceled += context => BlockInputCanceled?.Invoke();
                    _inputActions.Actions.LeftTrigger.performed += context => SpellInputPerformed?.Invoke();
                    _inputActions.Actions.LeftTrigger.canceled += context => SpellInputCanceled?.Invoke();
                    _inputActions.Actions.RightShoulder.performed += context => LightAttackInputPerformed?.Invoke();
                    _inputActions.Actions.RightTrigger.performed += context => HeavyAttackInputPerformed?.Invoke();

                    _inputActions.Actions.ButtonNorth.performed += context => SwapInputPerformed?.Invoke();
                    _inputActions.Actions.ButtonEast.performed += context => DodgeInputPerformed?.Invoke();
                    _inputActions.Actions.ButtonSouth.performed += context => ClimbInputPerformed?.Invoke();
                }
                else
                {
                    _inputActions.Actions.L3Press.performed -= context => KickInputPerformed?.Invoke();
                    _inputActions.Actions.R3Press.performed -= context => LockInputPerformed?.Invoke();
                    _inputActions.Actions.LeftShoulder.performed -= context => BlockInputPerformed?.Invoke();
                    _inputActions.Actions.LeftShoulder.canceled -= context => BlockInputCanceled?.Invoke();
                    _inputActions.Actions.LeftTrigger.performed -= context => SpellInputPerformed?.Invoke();
                    _inputActions.Actions.LeftTrigger.canceled -= context => SpellInputCanceled?.Invoke();
                    _inputActions.Actions.RightShoulder.performed -= context => LightAttackInputPerformed?.Invoke();
                    _inputActions.Actions.RightTrigger.performed -= context => HeavyAttackInputPerformed?.Invoke();

                    _inputActions.Actions.ButtonNorth.performed -= context => SwapInputPerformed?.Invoke();
                    _inputActions.Actions.ButtonEast.performed -= context => DodgeInputPerformed?.Invoke();
                    _inputActions.Actions.ButtonSouth.performed -= context => ClimbInputPerformed?.Invoke();
                }
            }
        }
    }
}