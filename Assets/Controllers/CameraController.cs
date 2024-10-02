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
    private Transform _pivot;
    private float _turnSmoothing = 0.1f;
    private float _mimimumAngle = -15f;
    private float _maximumAngle = 35f;
    private float _lookAngle;
    private float _tiltAngle;
    private float _smoothVelocityX;
    private float _smoothVelocityY;
    private float _smoothX;
    private float _smoothY;

    public Transform _CameraTransform { get; private set; }
    private Transform _target;

    public void Init(Transform target)
    {
        _target = target;
        _CameraTransform = Camera.main.transform;
        _transform = transform;
        _pivot = _CameraTransform.parent;

        _transform.position = _target.position;
    }

    public void Tick(float delta, float horizontal, float vertical, Transform lockOnTarget, bool lockedOn)
    {
        FollowTarget(delta);

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
        _tiltAngle = Mathf.Clamp(_tiltAngle, _mimimumAngle, _maximumAngle);
        _pivot.localRotation = Quaternion.Euler(_tiltAngle, 0, 0);

        if (lockedOn && lockOnTarget != null)
        {
            Vector3 targetDirection = lockOnTarget.position - _transform.position;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) targetDirection = _transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, delta * 9);

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
        Vector3 targetPosition = Vector3.Lerp(_transform.position, _target.position, speed);
        _transform.position = targetPosition;
    }
}
