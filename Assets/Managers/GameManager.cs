using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("PLAYER OBJECT REFERENCE")]
    [SerializeField] private GameObject _playerObject;
    private CharacterController _characterController;
    private PlayerInputManager _playerInputManager;
    private PlayerDataManager _playerDataManager;
    private PlayerMovement _playerMovement;
    private PlayerDash _playerDash;
    private Health _playerHealth;

    private void Awake()
    {
        _characterController = _playerObject.GetComponent<CharacterController>();
        _playerInputManager = _playerObject.GetComponent<PlayerInputManager>();
        _playerDataManager = _playerObject.GetComponent<PlayerDataManager>();
        _playerMovement = _playerObject.GetComponent<PlayerMovement>();
        _playerHealth = _playerObject.GetComponent<Health>();
    }

    private void Start()
    {
        _playerHealth.Death += GameManager_Death;
    }

    private void GameManager_Death(float deathTime)
    {

    }
}
