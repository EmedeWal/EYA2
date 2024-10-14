using UnityEngine;

public class LockTarget : MonoBehaviour
{
    [Header("LOCK FOCUS")]
    [SerializeField] private Transform _lockPoint;
    [SerializeField] private Transform _center;

    public Transform LockPoint => _lockPoint; 
    public Transform Center => _center;

    public Health Health { get; private set; }

    public void Init()
    {
        Health = GetComponent<Health>();
    }
}
