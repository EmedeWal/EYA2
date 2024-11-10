using UnityEngine;

public class CameraController : SingletonBase
{
    #region Singleton
    public static CameraController Instance;

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

    private Transform _transform;

    [Header("MOVEMENT")]
    [SerializeField] private float _followSpeed = 6f;

    [Header("ROTATION")]
    [SerializeField] private float _rotationSpeed = 1.5f;

    [Header("PANNING")]
    [SerializeField] private float _maximumAngle = 35f;
    [SerializeField] private float _mimimumAngle = -15f;
    private Transform _pivot;
    private float _turnSmoothing = 0.1f;
    private float _lookAngle;
    private float _tiltAngle;
    private float _smoothVelocityX;
    private float _smoothVelocityY;
    private float _smoothX;
    private float _smoothY;

    public Transform _CameraTransform { get; private set; }
    private Transform _followTarget;

    public void Init(Transform followTarget)
    {
        _followTarget = followTarget;
        _CameraTransform = Camera.main.transform;
        _transform = transform;
        _pivot = _CameraTransform.parent;

        _transform.SetPositionAndRotation(_followTarget.position, _followTarget.rotation);
        _lookAngle = _transform.rotation.eulerAngles.y;
    }

    public void FixedTick(float deltaTime, Transform lockTarget, Vector2 stickInput)
    {
        FollowTarget(deltaTime);

        float horizontal = stickInput.x;
        float vertical = stickInput.y;

        if (_turnSmoothing > 0)
        {
            _smoothX = Mathf.SmoothDamp(_smoothX, horizontal, ref _smoothVelocityX, _turnSmoothing);
            _smoothY = Mathf.SmoothDamp(_smoothY, vertical, ref _smoothVelocityY, _turnSmoothing);
        }
        else
        {
            _smoothX = horizontal;
            _smoothY = vertical;
        }

        _tiltAngle -= _smoothY * _rotationSpeed;
        _pivot.localRotation = Quaternion.Euler(_tiltAngle, 0, 0);
        _tiltAngle = Mathf.Clamp(_tiltAngle, _mimimumAngle, _maximumAngle);

        if (lockTarget != null)
        {
            Vector3 targetDirection = lockTarget.position - _transform.position;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, deltaTime * 9);

            _lookAngle = _transform.eulerAngles.y;
        }
        else
        {
            _lookAngle += _smoothX * _rotationSpeed;
            _transform.rotation = Quaternion.Euler(0, _lookAngle, 0);
        }
    }

    private void FollowTarget(float delta)
    {
        float speed = delta * _followSpeed;
        Vector3 targetPosition = Vector3.Lerp(_transform.position, _followTarget.position, speed);
        _transform.position = targetPosition;
    }
}