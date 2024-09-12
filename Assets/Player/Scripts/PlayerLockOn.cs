using System.Collections;
using UnityEngine;

public class PlayerLockOn : MonoBehaviour
{
    private Transform _transform;

    private PlayerInputHandler _inputHandler;
    private PlayerDataManager _dataManager;
    private CameraManager _cameraManager;
    private Camera _camera;
    private LayerMask _damageColliderLayer;
    private bool _lockedOn;

    [Header("VARIABLES")]
    [SerializeField] private float _lockOnRadius = 15f;
    [SerializeField] private float _viewThreshold = 0.75f;

    [Header("UI")]
    [SerializeField] private GameObject _lockOnMarker;

    private void Awake()
    {
        _transform = GetComponent<Transform>();

        _inputHandler = GetComponent<PlayerInputHandler>();
        _dataManager = GetComponent<PlayerDataManager>();
        _cameraManager = CameraManager.Instance;
        _camera = Camera.main;  

        _damageColliderLayer = LayerMask.GetMask("DamageCollider");

        _lockOnMarker.SetActive(false); 
    }

    private void OnEnable()
    {
        _inputHandler.LockOnInputPerformed += PlayerLockOn_LockOnInputPerformed;
    }

    private void OnDisable()
    {
        _inputHandler.LockOnInputPerformed -= PlayerLockOn_LockOnInputPerformed;
    }

    private void PlayerLockOn_LockOnInputPerformed()
    {
        if (_lockedOn)
        {
            EnableLockOnMarker(null, false);
            _dataManager.LockOnData.LockOnTarget = null;
            _lockedOn = false;
        }
        else
        {
            Transform target = FindLockOnTarget();
            if (target != null)
            {
                _lockedOn = true;
                _dataManager.LockOnData.LockOnTarget = target;
                EnableLockOnMarker(target, true);
            }
        }

        _dataManager.LockOnData.LockedOn = _lockedOn;
    }

    private void EnableLockOnMarker(Transform target, bool enable)
    {
        _lockOnMarker.SetActive(enable);
        
        if (enable)
        {
            StartCoroutine(UpdateLockOnMarkerPositionCoroutine(target));
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private Transform FindLockOnTarget()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(_transform.position, _lockOnRadius, _damageColliderLayer);
        Transform bestTarget = null;
        float bestScore = Mathf.Infinity;

        foreach (Collider target in potentialTargets)
        {
            if (target.TryGetComponent(out LockOnTarget lockOnTarget))
            {
                Transform targetTransform = lockOnTarget.GetCenter();
                Vector3 toTarget = targetTransform.position - _cameraManager._CameraTransform.position;
                toTarget.Normalize();
                float alignment = Vector3.Dot(_cameraManager._CameraTransform.forward, toTarget);

                if (alignment > _viewThreshold)
                {
                    float distance = Vector3.Distance(_transform.position, targetTransform.position);
                    float score = distance * (1 - alignment);

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestTarget = targetTransform;
                    }
                }
            }
        }

        return bestTarget;
    }

    private IEnumerator UpdateLockOnMarkerPositionCoroutine(Transform target)
    {
        while (true)
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(target.position);
            _lockOnMarker.transform.position = screenPosition;

            yield return null;
        }
    }
}
