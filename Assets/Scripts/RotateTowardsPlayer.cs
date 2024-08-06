using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    private Transform playerCameraTransform;

    private void Start()
    {
        playerCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 directionToCamera = playerCameraTransform.position - transform.position;
        Vector3 targetDirection = -directionToCamera;

        transform.rotation = Quaternion.LookRotation(targetDirection);
    }
}
