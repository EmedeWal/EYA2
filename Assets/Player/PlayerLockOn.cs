using UnityEngine;

public class PlayerLockOn : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private PlayerDataManager _dataManager;

    private Transform _lockOnTargetTransform;
    private bool _lockedOn;

    private void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _dataManager = GetComponent<PlayerDataManager>();
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

    }
}
