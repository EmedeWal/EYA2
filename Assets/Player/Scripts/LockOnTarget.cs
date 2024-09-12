using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [SerializeField] private Transform _center;

    public Transform GetCenter()
    {
        return _center;
    }
}
