using System.Collections;
using UnityEngine;

public class TrackingProjectile : MonoBehaviour
{
    [Header("MOBILITY")]
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotationSpeed = 100;

    [Header("EXPLOSION")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float _detonationRange = 5f;
    [SerializeField] private float _detonationDelay = 1f;
    [SerializeField] private float _activationTime = 1f;

    private Transform _playerTransform;
    private bool _active = false;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        Invoke(nameof(Activate), _activationTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_active) InstantiateExplosion();
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, _playerTransform.position);
    }

    private IEnumerator MoveToPlayer()
    {
        while (true)
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.forward);

            Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;
            directionToPlayer.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);

            if (DistanceToPlayer() <= _detonationRange)
            {
                Invoke(nameof(InstantiateExplosion), _detonationDelay);
                StopAllCoroutines();
            }

            yield return null;
        }
    }

    private void InstantiateExplosion()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void Activate()
    {
        _active = true;
        StartCoroutine(MoveToPlayer());
    }
}
