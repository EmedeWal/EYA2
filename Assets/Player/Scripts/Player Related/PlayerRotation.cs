using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // References
    private PlayerStateManager _stateManager;
    private PlayerDataManager _dataManager;

    [Header("ROTATION")]
    [SerializeField] private float _rotationSpeed = 15f;

    [Header("AIM ASSIST")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _assistRange = 5f;
    [SerializeField] private float _maxAngle = 60f;

    private Vector3 _rotationDirection;

    private void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _dataManager = GetComponent<PlayerDataManager>();
    } 

    private void Update()
    {
        GetDirection();
        DetermineBehavior();
    }

    private void GetDirection()
    {
        Vector2 directionValue = _dataManager.GetDirection();
        _rotationDirection = new (directionValue.x, 0, directionValue.y);
    }

    private void DetermineBehavior()
    {
        if (_stateManager.IsCharging())
        {
            RotateTowardsNearestEnemy();
        }
        else if (_stateManager.CanRotate())
        {
            DefaultRotation();
        }
    }

    private void DefaultRotation()
    {
        if (_rotationDirection == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(_rotationDirection);
        float rotationSpeed = _rotationSpeed * Time.deltaTime;

        Rotation(lookRotation, rotationSpeed);
    }

    private void Rotation(Quaternion lookRotation, float rotationSpeed)
    {
        if (_stateManager.CanRotate())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed);
        }
    }

    private void RotateTowardsNearestEnemy()
    {
        Collider[] enemies = EnemiesInRange();

        Transform closestEnemy = null;
        float smallestAngle = float.MaxValue;

        Vector3 worldDirection = CalculateDirection().normalized;
        Vector3 playerForwardInputAdjusted = Quaternion.LookRotation(worldDirection) * Vector3.forward;

        foreach (Collider enemy in enemies)
        {
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(playerForwardInputAdjusted, directionToEnemy);

            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null && smallestAngle <= _maxAngle)
        {
            Vector3 targetDirection = closestEnemy.position - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            float rotationSpeed = _rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
        else
        {
            DefaultRotation();
        }
    }


    private Collider[] EnemiesInRange()
    {
        return Physics.OverlapSphere(transform.position, _assistRange, _enemyLayer);
    }

    private Vector3 CalculateDirection()
    {
        if (_rotationDirection == Vector3.zero)
        {
            return transform.forward;
        }

        return _rotationDirection;
    }
}
