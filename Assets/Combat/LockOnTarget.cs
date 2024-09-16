using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [Header("LOCK ON FOCUS")]
    [SerializeField] private Transform _lockOnPoint;
    [SerializeField] private Transform _center;

    public Transform _LockOnPoint => _lockOnPoint; public Transform _Center => _center;

    public Health _Health { get; private set; }

    private void Awake()
    {
        _Health = GetComponent<Health>();       
    }
}
