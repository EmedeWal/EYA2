using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    public static CameraManager Instance;

    void Awake()
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

    public Transform Target;
    public float FollowSpeed = 9f;
    public float RotationSpeed = 2f;
    public bool LockedOn = false;

    Transform _cameraTransform;
    Transform _transform;
    Transform _pivot;
    float _turnSmoothing = 0.1f;
    float _mimimumAngle = -35f;
    float _maximumAngle = 35f;
    float _lookAngle;
    float _tiltAngle;
    float _smoothVelocityX;
    float _smoothVelocityY;
    float _smoothX;
    float _smoothY;

    public void Initialize(Transform target)
    {
        Target = target;
        RotationSpeed /= 100;
        _cameraTransform = Camera.main.transform;
        _transform = transform;
        _pivot = _cameraTransform.parent;
    }

    public void OnUpdate(float delta, float horizontal, float vertical)
    {
        FollowTarget(delta);
        HandleRotation(delta, horizontal, vertical);
    }

    void FollowTarget(float delta)
    {
        float speed = delta * FollowSpeed;
        Vector3 targetPosition = Vector3.Lerp(_transform.position, Target.position, speed);
        _transform.position = targetPosition;
    }

    void HandleRotation(float delta, float horizontal, float vertical)
    {
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

        if (LockedOn)
        {

        }

        _lookAngle += _smoothX * RotationSpeed;
        _transform.rotation = Quaternion.Euler(0, _lookAngle, 0);

        _tiltAngle -= _smoothY * RotationSpeed;
        _tiltAngle = Mathf.Clamp(_tiltAngle, _mimimumAngle, _maximumAngle);
        _pivot.localRotation = Quaternion.Euler(_tiltAngle, 0, 0);
    }
}
