//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Unity/Input System/InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace EmeWillem
{
    public partial class @InputActions: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""93ea7213-e431-4b35-846f-0bf251c854d3"",
            ""actions"": [
                {
                    ""name"": ""Left Stick"",
                    ""type"": ""Value"",
                    ""id"": ""4bc70efc-1447-4a84-8360-a4cdbc623c67"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Right Stick"",
                    ""type"": ""Value"",
                    ""id"": ""bf2637c3-8c02-4964-bb08-2d692dd243e2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""31ae35bb-765e-4474-8092-d32a25e625f7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""05ca8fd6-61e6-46a2-a67d-e76bc9708a83"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""125748ea-0889-459d-b72c-c42e842ea7be"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cc2c3446-cacd-436e-af0f-7cadfec095aa"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""675598de-424a-4541-96a2-6ea7a68954f0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""719ad53f-87bb-40cb-b05e-a392d4bba297"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32cbe835-15eb-4d12-a5fb-8af81493e2c9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbdd6924-d0c0-4822-a6c6-2196345c06ee"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Actions"",
            ""id"": ""c677d909-742b-41c9-8816-ca21899da168"",
            ""actions"": [
                {
                    ""name"": ""L3 Press"",
                    ""type"": ""Button"",
                    ""id"": ""59c0b63f-5dfc-4739-b9b2-1b39fcf01302"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""R3 Press"",
                    ""type"": ""Button"",
                    ""id"": ""67317a6f-8f6a-4872-b118-2017616622f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Left Shoulder"",
                    ""type"": ""Value"",
                    ""id"": ""1aac6eed-0147-40e5-b2fd-2775e5c55c65"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Left Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""c9b86052-a08c-43b0-9a5f-6e1c2c35f4b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right Shoulder"",
                    ""type"": ""Button"",
                    ""id"": ""633c81d8-9883-482e-b625-e3e772516d40"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""75de8292-2823-4fe4-89d7-7aac021f5864"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Button North"",
                    ""type"": ""Button"",
                    ""id"": ""0cb03d7d-2fbe-4555-99f6-2c300fb77492"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Button East"",
                    ""type"": ""Button"",
                    ""id"": ""6a740f7d-2b49-4863-8d0e-a9da5697cc71"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Button South"",
                    ""type"": ""Button"",
                    ""id"": ""a6481634-a8ec-437e-822d-17c81e46420e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Button West"",
                    ""type"": ""Button"",
                    ""id"": ""14ef6eba-e11a-4b77-913d-1b8354bb8e30"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Options"",
                    ""type"": ""Button"",
                    ""id"": ""fa1345a0-69ab-4cb8-ac38-d01b02b99cd2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""96e0ed73-b636-4768-8b32-69a688bf4a7a"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button North"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fea750d0-5786-4996-882d-6c9d3723cdbe"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button North"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75df22c5-181f-4806-8eaf-8a933a6ba22b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button West"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09b461ec-ee69-4e79-bc31-f6e562f40bc8"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button West"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e926b72-3c47-4133-889c-e8e74a2beef2"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""R3 Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a662151-26e5-4f2f-8b07-ea1c23ecc4c2"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""R3 Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3e05a26-3dcf-4139-986b-82f70015f061"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""L3 Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c64f841-e311-4f9a-ad39-d7f6acd76a7c"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""L3 Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49ff0107-5df9-4f04-a70b-04ebfd7989b2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7d82ae6-80d5-414b-8cc4-b05f71f097f6"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e9dea88-2f21-45b6-af63-48b28b97003c"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Shoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8217b91d-75bb-4f2f-809e-616c6b3b7360"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Shoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25da4379-519d-416e-9f16-a6f6ce934328"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c8e4ed1-66d2-457b-a0ff-b0394883a0d3"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d74cfd98-1665-4d97-b310-7168808883c0"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4cc052ef-514a-4bf7-9f61-da183955ef12"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b044e8a-4225-4e57-b15b-e0ff32b9fd90"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button South"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4056f26d-c007-4246-8616-0312266b1131"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button South"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf358fec-66da-4ae2-8d22-95a76ab329b3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb15c798-469c-48b6-80ed-a411cad6d646"",
                    ""path"": ""<Gamepad>/{Menu}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20ff7c76-dd34-4f4f-8fe1-f0be83da68ce"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Shoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0103c3b7-3ad4-4b18-a823-7f8e745cd942"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Shoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Movement
            m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
            m_Movement_LeftStick = m_Movement.FindAction("Left Stick", throwIfNotFound: true);
            m_Movement_RightStick = m_Movement.FindAction("Right Stick", throwIfNotFound: true);
            // Actions
            m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
            m_Actions_L3Press = m_Actions.FindAction("L3 Press", throwIfNotFound: true);
            m_Actions_R3Press = m_Actions.FindAction("R3 Press", throwIfNotFound: true);
            m_Actions_LeftShoulder = m_Actions.FindAction("Left Shoulder", throwIfNotFound: true);
            m_Actions_LeftTrigger = m_Actions.FindAction("Left Trigger", throwIfNotFound: true);
            m_Actions_RightShoulder = m_Actions.FindAction("Right Shoulder", throwIfNotFound: true);
            m_Actions_RightTrigger = m_Actions.FindAction("Right Trigger", throwIfNotFound: true);
            m_Actions_ButtonNorth = m_Actions.FindAction("Button North", throwIfNotFound: true);
            m_Actions_ButtonEast = m_Actions.FindAction("Button East", throwIfNotFound: true);
            m_Actions_ButtonSouth = m_Actions.FindAction("Button South", throwIfNotFound: true);
            m_Actions_ButtonWest = m_Actions.FindAction("Button West", throwIfNotFound: true);
            m_Actions_Options = m_Actions.FindAction("Options", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Movement
        private readonly InputActionMap m_Movement;
        private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
        private readonly InputAction m_Movement_LeftStick;
        private readonly InputAction m_Movement_RightStick;
        public struct MovementActions
        {
            private @InputActions m_Wrapper;
            public MovementActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @LeftStick => m_Wrapper.m_Movement_LeftStick;
            public InputAction @RightStick => m_Wrapper.m_Movement_RightStick;
            public InputActionMap Get() { return m_Wrapper.m_Movement; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
            public void AddCallbacks(IMovementActions instance)
            {
                if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
                @LeftStick.started += instance.OnLeftStick;
                @LeftStick.performed += instance.OnLeftStick;
                @LeftStick.canceled += instance.OnLeftStick;
                @RightStick.started += instance.OnRightStick;
                @RightStick.performed += instance.OnRightStick;
                @RightStick.canceled += instance.OnRightStick;
            }

            private void UnregisterCallbacks(IMovementActions instance)
            {
                @LeftStick.started -= instance.OnLeftStick;
                @LeftStick.performed -= instance.OnLeftStick;
                @LeftStick.canceled -= instance.OnLeftStick;
                @RightStick.started -= instance.OnRightStick;
                @RightStick.performed -= instance.OnRightStick;
                @RightStick.canceled -= instance.OnRightStick;
            }

            public void RemoveCallbacks(IMovementActions instance)
            {
                if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMovementActions instance)
            {
                foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MovementActions @Movement => new MovementActions(this);

        // Actions
        private readonly InputActionMap m_Actions;
        private List<IActionsActions> m_ActionsActionsCallbackInterfaces = new List<IActionsActions>();
        private readonly InputAction m_Actions_L3Press;
        private readonly InputAction m_Actions_R3Press;
        private readonly InputAction m_Actions_LeftShoulder;
        private readonly InputAction m_Actions_LeftTrigger;
        private readonly InputAction m_Actions_RightShoulder;
        private readonly InputAction m_Actions_RightTrigger;
        private readonly InputAction m_Actions_ButtonNorth;
        private readonly InputAction m_Actions_ButtonEast;
        private readonly InputAction m_Actions_ButtonSouth;
        private readonly InputAction m_Actions_ButtonWest;
        private readonly InputAction m_Actions_Options;
        public struct ActionsActions
        {
            private @InputActions m_Wrapper;
            public ActionsActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @L3Press => m_Wrapper.m_Actions_L3Press;
            public InputAction @R3Press => m_Wrapper.m_Actions_R3Press;
            public InputAction @LeftShoulder => m_Wrapper.m_Actions_LeftShoulder;
            public InputAction @LeftTrigger => m_Wrapper.m_Actions_LeftTrigger;
            public InputAction @RightShoulder => m_Wrapper.m_Actions_RightShoulder;
            public InputAction @RightTrigger => m_Wrapper.m_Actions_RightTrigger;
            public InputAction @ButtonNorth => m_Wrapper.m_Actions_ButtonNorth;
            public InputAction @ButtonEast => m_Wrapper.m_Actions_ButtonEast;
            public InputAction @ButtonSouth => m_Wrapper.m_Actions_ButtonSouth;
            public InputAction @ButtonWest => m_Wrapper.m_Actions_ButtonWest;
            public InputAction @Options => m_Wrapper.m_Actions_Options;
            public InputActionMap Get() { return m_Wrapper.m_Actions; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
            public void AddCallbacks(IActionsActions instance)
            {
                if (instance == null || m_Wrapper.m_ActionsActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_ActionsActionsCallbackInterfaces.Add(instance);
                @L3Press.started += instance.OnL3Press;
                @L3Press.performed += instance.OnL3Press;
                @L3Press.canceled += instance.OnL3Press;
                @R3Press.started += instance.OnR3Press;
                @R3Press.performed += instance.OnR3Press;
                @R3Press.canceled += instance.OnR3Press;
                @LeftShoulder.started += instance.OnLeftShoulder;
                @LeftShoulder.performed += instance.OnLeftShoulder;
                @LeftShoulder.canceled += instance.OnLeftShoulder;
                @LeftTrigger.started += instance.OnLeftTrigger;
                @LeftTrigger.performed += instance.OnLeftTrigger;
                @LeftTrigger.canceled += instance.OnLeftTrigger;
                @RightShoulder.started += instance.OnRightShoulder;
                @RightShoulder.performed += instance.OnRightShoulder;
                @RightShoulder.canceled += instance.OnRightShoulder;
                @RightTrigger.started += instance.OnRightTrigger;
                @RightTrigger.performed += instance.OnRightTrigger;
                @RightTrigger.canceled += instance.OnRightTrigger;
                @ButtonNorth.started += instance.OnButtonNorth;
                @ButtonNorth.performed += instance.OnButtonNorth;
                @ButtonNorth.canceled += instance.OnButtonNorth;
                @ButtonEast.started += instance.OnButtonEast;
                @ButtonEast.performed += instance.OnButtonEast;
                @ButtonEast.canceled += instance.OnButtonEast;
                @ButtonSouth.started += instance.OnButtonSouth;
                @ButtonSouth.performed += instance.OnButtonSouth;
                @ButtonSouth.canceled += instance.OnButtonSouth;
                @ButtonWest.started += instance.OnButtonWest;
                @ButtonWest.performed += instance.OnButtonWest;
                @ButtonWest.canceled += instance.OnButtonWest;
                @Options.started += instance.OnOptions;
                @Options.performed += instance.OnOptions;
                @Options.canceled += instance.OnOptions;
            }

            private void UnregisterCallbacks(IActionsActions instance)
            {
                @L3Press.started -= instance.OnL3Press;
                @L3Press.performed -= instance.OnL3Press;
                @L3Press.canceled -= instance.OnL3Press;
                @R3Press.started -= instance.OnR3Press;
                @R3Press.performed -= instance.OnR3Press;
                @R3Press.canceled -= instance.OnR3Press;
                @LeftShoulder.started -= instance.OnLeftShoulder;
                @LeftShoulder.performed -= instance.OnLeftShoulder;
                @LeftShoulder.canceled -= instance.OnLeftShoulder;
                @LeftTrigger.started -= instance.OnLeftTrigger;
                @LeftTrigger.performed -= instance.OnLeftTrigger;
                @LeftTrigger.canceled -= instance.OnLeftTrigger;
                @RightShoulder.started -= instance.OnRightShoulder;
                @RightShoulder.performed -= instance.OnRightShoulder;
                @RightShoulder.canceled -= instance.OnRightShoulder;
                @RightTrigger.started -= instance.OnRightTrigger;
                @RightTrigger.performed -= instance.OnRightTrigger;
                @RightTrigger.canceled -= instance.OnRightTrigger;
                @ButtonNorth.started -= instance.OnButtonNorth;
                @ButtonNorth.performed -= instance.OnButtonNorth;
                @ButtonNorth.canceled -= instance.OnButtonNorth;
                @ButtonEast.started -= instance.OnButtonEast;
                @ButtonEast.performed -= instance.OnButtonEast;
                @ButtonEast.canceled -= instance.OnButtonEast;
                @ButtonSouth.started -= instance.OnButtonSouth;
                @ButtonSouth.performed -= instance.OnButtonSouth;
                @ButtonSouth.canceled -= instance.OnButtonSouth;
                @ButtonWest.started -= instance.OnButtonWest;
                @ButtonWest.performed -= instance.OnButtonWest;
                @ButtonWest.canceled -= instance.OnButtonWest;
                @Options.started -= instance.OnOptions;
                @Options.performed -= instance.OnOptions;
                @Options.canceled -= instance.OnOptions;
            }

            public void RemoveCallbacks(IActionsActions instance)
            {
                if (m_Wrapper.m_ActionsActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IActionsActions instance)
            {
                foreach (var item in m_Wrapper.m_ActionsActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_ActionsActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public ActionsActions @Actions => new ActionsActions(this);
        public interface IMovementActions
        {
            void OnLeftStick(InputAction.CallbackContext context);
            void OnRightStick(InputAction.CallbackContext context);
        }
        public interface IActionsActions
        {
            void OnL3Press(InputAction.CallbackContext context);
            void OnR3Press(InputAction.CallbackContext context);
            void OnLeftShoulder(InputAction.CallbackContext context);
            void OnLeftTrigger(InputAction.CallbackContext context);
            void OnRightShoulder(InputAction.CallbackContext context);
            void OnRightTrigger(InputAction.CallbackContext context);
            void OnButtonNorth(InputAction.CallbackContext context);
            void OnButtonEast(InputAction.CallbackContext context);
            void OnButtonSouth(InputAction.CallbackContext context);
            void OnButtonWest(InputAction.CallbackContext context);
            void OnOptions(InputAction.CallbackContext context);
        }
    }
}
