using System.Collections;
using UnityEngine;

public class PlayerLockOn : MonoBehaviour
{
    private Transform _transform;

    private PlayerInputHandler _inputHandler;
    private LockOnMarker _lockOnMarker;
    private Camera _camera;
    private LayerMask _damageColliderLayer;
    private bool _lockedOn;

    private GameObject _lockOnMarkerObject;
    private Transform _lockOnMarkerTransform;
    private Transform _cameraTransform;

    [Header("VARIABLES")]
    [SerializeField] private float _lockOnRadius = 15f;
    [SerializeField] private float _viewThreshold = 0.75f;
    [SerializeField] private float _stickSensitivity = 0.50f;

    private LockOnTarget _lockOnTarget;
    private Transform _lockOnTargetCenter;
    private Transform _lockOnMarkerPoint;

    public Transform Target => _lockOnTargetCenter;

    public delegate void LockedOnDelegate(Transform target);
    public event LockedOnDelegate LockedOn;

    public void Init()
    {
        _transform = GetComponent<Transform>();

        _inputHandler = GetComponent<PlayerInputHandler>();
        _lockOnMarker = LockOnMarker.Instance;
        _camera = Camera.main;

        _damageColliderLayer = LayerMask.GetMask("DamageCollider");

        _lockOnMarkerObject = _lockOnMarker.gameObject;
        _lockOnMarkerTransform = _lockOnMarker.transform;
        _cameraTransform = _camera.transform;

        _lockOnMarkerObject.SetActive(false);

        _inputHandler.LockOnInputPerformed += PlayerLockOn_LockOnInputPerformed;
    }

    public void Tick()
    {
        if (!_lockedOn) return;

        float rightStickX = _inputHandler._RightStickX;
        float rightStickY = _inputHandler._RightStickY;

        if (Mathf.Abs(rightStickX) > _stickSensitivity || Mathf.Abs(rightStickY) > _stickSensitivity)
        {
            SwapLockOnTarget(rightStickX, rightStickY);
        }
    }

    public void Cleanup()
    {
        _inputHandler.LockOnInputPerformed -= PlayerLockOn_LockOnInputPerformed;
    }

    private void SwapLockOnTarget(float rightStickX, float rightStickY)
    {
        Collider[] potentialTargets = Physics.OverlapSphere(_transform.position, _lockOnRadius, _damageColliderLayer);
        LockOnTarget bestTarget = null;
        float bestScore = Mathf.NegativeInfinity;

        Vector2 stickDirection = new Vector2(rightStickX, rightStickY).normalized;

        foreach (Collider target in potentialTargets)
        {
            if (target.TryGetComponent(out LockOnTarget lockOnTarget))
            {
                Transform targetTransform = lockOnTarget._Center;
                Vector3 toTarget = targetTransform.position - _cameraTransform.position;
                toTarget.Normalize();
                Vector2 screenDirection = new Vector2(toTarget.x, toTarget.z).normalized;
                float alignment = Vector2.Dot(stickDirection, screenDirection);

                if (alignment > bestScore)
                {
                    bestScore = alignment;
                    bestTarget = lockOnTarget;
                }
            }
        }

        if (bestTarget != null && bestTarget._Center != _lockOnTargetCenter)
        {
            EnableLockOn(bestTarget);
        }
    }

    private void PlayerLockOn_LockOnInputPerformed()
    {
        if (_lockOnTargetCenter != null)
        {
            DisableLockOn();
        }
        else
        {
            LockOnTarget target = FindLockOnTarget();
            if (target != null)
            {
                EnableLockOn(target);
            }
        }
    }

    private void DisableLockOn()
    {
        if (_lockOnTarget != null)
        {
            _lockOnTarget._Health.ValueExhausted -= PlayerLockOn_ValueExhausted;
        }

        _lockOnTargetCenter = null;
        _lockedOn = false;

        UpdateData(null);
        StopAllCoroutines();
    }

    private void EnableLockOn(LockOnTarget target)
    {
        _lockedOn = true;
        _lockOnTarget = target;
        _lockOnTargetCenter = target._Center;
        _lockOnMarkerPoint = target._LockOnPoint;

        _lockOnMarker.SetLockOnTarget(target);

        _lockOnTarget._Health.ValueExhausted += PlayerLockOn_ValueExhausted;

        StopAllCoroutines();
        UpdateData(_lockOnTargetCenter);
        StartCoroutine(UpdateLockOnMarkerPositionCoroutine());
    }

    private void UpdateData(Transform target)
    {
        OnLockedOn(target);
        _lockOnMarkerObject.SetActive(_lockedOn);
    }

    private void PlayerLockOn_ValueExhausted(GameObject deathObject)
    {
        DisableLockOn();
        StartCoroutine(FindNewLockOnTarget());

        IEnumerator FindNewLockOnTarget()
        {
            yield return new WaitForEndOfFrame();

            LockOnTarget target = FindLockOnTarget();
            if (target != null)
            {
                EnableLockOn(target);
            }
        }
    }

    private LockOnTarget FindLockOnTarget()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(_transform.position, _lockOnRadius, _damageColliderLayer);
        LockOnTarget bestTarget = null;
        float bestScore = Mathf.Infinity;

        foreach (Collider target in potentialTargets)
        {
            if (target.TryGetComponent(out LockOnTarget lockOnTarget))
            {
                Transform targetTransform = lockOnTarget._Center;
                Vector3 toTarget = targetTransform.position - _cameraTransform.position;
                toTarget.Normalize();
                float alignment = Vector3.Dot(_cameraTransform.forward, toTarget);

                if (alignment > _viewThreshold)
                {
                    float distance = Vector3.Distance(_transform.position, targetTransform.position);
                    float score = distance * (1 - alignment);

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestTarget = lockOnTarget;
                    }
                }
            }
        }

        return bestTarget;
    }
    private IEnumerator UpdateLockOnMarkerPositionCoroutine()
    {
        while (_lockOnTarget != null)
        {
            _lockOnMarkerTransform.position = _lockOnMarkerPoint.position;
            Vector3 directionToCamera = _cameraTransform.position - _lockOnMarkerTransform.position;
            Vector3 targetDirection = -directionToCamera;
            _lockOnMarkerTransform.rotation = Quaternion.LookRotation(targetDirection);
            yield return null;
        }
    }

    private void OnLockedOn(Transform target)
    {
        LockedOn?.Invoke(target);
    }
}
