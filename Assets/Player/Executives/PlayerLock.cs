using System.Collections;
using UnityEngine;

public class PlayerLock : MonoBehaviour
{
    private Transform _transform;

    private PlayerInputHandler _inputHandler;
    private LayerMask _targetLayer;

    private LockMarker _lockMarker;
    private GameObject _lockMarkerObject;
    private Transform _lockMarkerTransform;

    private Transform _cameraTransform;
    private Camera _camera;

    [Header("VARIABLES")]
    [SerializeField] private float _lockRadius = 15f;
    [SerializeField] private float _viewThreshold = 0.75f;
    [SerializeField] private float _stickSensitivity = 0.50f;

    private LockTarget _lockTarget = null;

    public Transform Target { get; private set; } = null;

    public delegate void LockedDelegate(Transform target);
    public event LockedDelegate Locked;

    public void Init()
    {
        _transform = GetComponent<Transform>();

        _inputHandler = GetComponent<PlayerInputHandler>();

        _targetLayer = LayerMask.GetMask("DamageCollider");

        _lockMarker = LockMarker.Instance;
        _lockMarkerObject = _lockMarker.gameObject;
        _lockMarkerTransform = _lockMarker.transform;

        _camera = Camera.main;
        _cameraTransform = _camera.transform;

        _lockMarkerObject.SetActive(false);

        CreatureManager.CreatureDeath += PlayerLock_CreatureDeath;
        _inputHandler.LockOnInputPerformed += PlayerLockOn_LockOnInputPerformed;
    }

    public void Cleanup()
    {
        CreatureManager.CreatureDeath -= PlayerLock_CreatureDeath;
        _inputHandler.LockOnInputPerformed -= PlayerLockOn_LockOnInputPerformed;
    }

    private void PlayerLock_CreatureDeath(CreatureAI creature)
    {
        if (_lockTarget != null && _lockTarget == creature.LockTarget)
        {
            DisableLock();
            StartCoroutine(FindLockOnTarget());
        }
    }

    private void PlayerLockOn_LockOnInputPerformed()
    {
        if (_lockTarget != null)
        {
            DisableLock();
        }
        else
        {
            StartCoroutine(FindLockOnTarget());
        }
    }

    private void DisableLock()
    {
        _lockMarkerObject.SetActive(false);
        StopAllCoroutines();
        _lockTarget = null;
        OnLockedOn(null);
        Target = null;
    }

    private void EnableLock(LockTarget lockTarget)
    {
        _lockTarget = lockTarget;
        Target = _lockTarget.Center;
        OnLockedOn(_lockTarget.Center);
        StartCoroutine(LockCoroutine());
        _lockMarkerObject.SetActive(true);
        _lockMarker.SetLockOnTarget(lockTarget);
    }

    private void ManageLockMarker()
    {
        _lockMarkerTransform.position = _lockTarget.LockPoint.position;
        Vector3 directionToCamera = _cameraTransform.position - _lockMarkerTransform.position;
        Vector3 targetDirection = -directionToCamera;
        _lockMarkerTransform.rotation = Quaternion.LookRotation(targetDirection);
    }

    private void ManageLockSwap()
    {
        float rightStickX = _inputHandler.RightStickX;
        float rightStickY = _inputHandler.RightStickY;

        if (Mathf.Abs(rightStickX) > _stickSensitivity || Mathf.Abs(rightStickY) > _stickSensitivity)
        {
            StartCoroutine(SwapLockOnTarget(rightStickX, rightStickY));
        }
    }

    private void OnLockedOn(Transform target)
    {
        Locked?.Invoke(target);
    }

    private IEnumerator LockCoroutine()
    {
        while (_lockTarget != null)
        {
            yield return null;
            ManageLockMarker();
            ManageLockSwap();
        }
    }

    private IEnumerator FindLockOnTarget()
    {
        yield return new WaitForEndOfFrame();

        Collider[] potentialTargets = Physics.OverlapSphere(_transform.position, _lockRadius, _targetLayer);
        LockTarget bestTarget = null;
        float bestScore = Mathf.Infinity;

        foreach (Collider target in potentialTargets)
        {
            if (target.TryGetComponent(out LockTarget lockTarget))
            {
                Transform targetCenter = lockTarget.Center;
                Vector3 toTarget = targetCenter.position - _cameraTransform.position;
                toTarget.Normalize();
                float alignment = Vector3.Dot(_cameraTransform.forward, toTarget);

                if (alignment > _viewThreshold)
                {
                    float distance = Vector3.Distance(_transform.position, targetCenter.position);
                    float score = distance * (1 - alignment);

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestTarget = lockTarget;
                    }
                }
            }
        }

        if (bestTarget != null)
        {
            EnableLock(bestTarget);
        }
    }

    private IEnumerator SwapLockOnTarget(float rightStickX, float rightStickY)
    {
        yield return new WaitForEndOfFrame();

        Collider[] potentialTargets = Physics.OverlapSphere(_transform.position, _lockRadius, _targetLayer);
        LockTarget bestTarget = null;
        float bestScore = Mathf.NegativeInfinity;

        Vector2 stickDirection = new Vector2(rightStickX, rightStickY).normalized;

        foreach (Collider target in potentialTargets)
        {
            if (target.TryGetComponent(out LockTarget lockOnTarget))
            {
                Transform targetTransform = lockOnTarget.Center;
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

        if (bestTarget != null && bestTarget.Center != _lockTarget.Center)
        {
            EnableLock(bestTarget);
        }
    }
}