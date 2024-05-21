using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform _target;

    public delegate void Teleporter_TeleportedPlayer();
    public static event Teleporter_TeleportedPlayer TeleportedPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            controller.enabled = false;
            other.transform.position = _target.position;
            other.transform.rotation = _target.rotation;
            controller.enabled = true;

            TeleportedPlayer?.Invoke();
        }
    }
}
