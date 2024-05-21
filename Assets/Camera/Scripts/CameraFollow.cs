using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private Vector3 _offset = new (0f, 15f, -15f);
    [SerializeField] private float _smoothTime = 15f;

    private Transform _target;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        if (_target != null) transform.position = _target.position + _offset;
    }

    private void Update()
    {
        if (_target != null) FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = _target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothTime * Time.deltaTime);
    }
}
