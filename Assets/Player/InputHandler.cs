using UnityEngine;

public class InputHandler : MonoBehaviour
{
    CameraManager _cameraManager;
    StateManager _stateManager;
    float _delta;

    Vector2 _leftStickValue;
    float _leftStickX;
    float _leftStickY;

    Vector2 _rightStickValue;
    float _rightStickX;
    float _rightStickY;

    #region Input Setup
    InputActions _inputActions;

    void OnEnable()
    {
        _inputActions ??= new InputActions();

        _inputActions.PlayerMovement.LeftStick.performed += indexer => _leftStickValue = indexer.ReadValue<Vector2>();
        _inputActions.PlayerMovement.LeftStick.canceled += indexer => _leftStickValue = indexer.ReadValue<Vector2>();
        _inputActions.PlayerMovement.RightStick.performed += indexer => _rightStickValue = indexer.ReadValue<Vector2>();
        _inputActions.PlayerMovement.RightStick.canceled += indexer => _rightStickValue = indexer.ReadValue<Vector2>();

        _inputActions.Enable();
    }


    void OnDisable()
    {
        _inputActions.Disable();   
    }
    #endregion

    void Awake()
    {
        _cameraManager = CameraManager.Instance;    
        _cameraManager.Initialize(transform);
        _stateManager = GetComponent<StateManager>();
        _stateManager.Initialize();
    }

    void Update()
    {
        HandleAllInput();
        _delta = Time.deltaTime;
        _stateManager.OnUpdate(_delta);
    }

    void FixedUpdate()
    {
        _delta = Time.fixedDeltaTime;
        HandleAllInput();
        UpdateStateManager();
        _cameraManager.OnUpdate(_delta, _rightStickX, _rightStickY);
    }

    void UpdateStateManager()
    {
        _stateManager.Horizontal = _leftStickX;
        _stateManager.Vertical = _leftStickY;

        Vector3 horizontal = _stateManager.Horizontal * _cameraManager.transform.right;
        Vector3 vertical = _stateManager.Vertical * _cameraManager.transform.forward;
        _stateManager.MovementDirection = (horizontal + vertical).normalized;
        float movement = Mathf.Abs(_leftStickX) + Mathf.Abs(_leftStickY);
        _stateManager.MovementAmount = Mathf.Clamp01(movement);

        _stateManager.OnFixedUpdate(_delta);
    }

    void HandleAllInput()
    {
        HandleLeftStickInput();
        HandleRightStickInput();
    }

    void HandleLeftStickInput()
    {
        _leftStickX = _leftStickValue.x;
        _leftStickY = _leftStickValue.y;
    }

    void HandleRightStickInput()
    {
        _rightStickX = _rightStickValue.x;
        _rightStickY = _rightStickValue.y;
    }
}
