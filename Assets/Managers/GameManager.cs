using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _characterController = _playerObject.GetComponent<CharacterController>();
        _playerInputManager = _playerObject.GetComponent<PlayerInputManager>();
        _playerDataManager = _playerObject.GetComponent<PlayerDataManager>();
        _playerMovement = _playerObject.GetComponent<PlayerMovement>();
        _playerHealth = _playerObject.GetComponent<Health>();
    }
    #endregion

    [Header("PLAYER OBJECT REFERENCE")]
    [SerializeField] private GameObject _playerObject;
    private CharacterController _characterController;
    private PlayerInputManager _playerInputManager;
    private PlayerDataManager _playerDataManager;
    private PlayerMovement _playerMovement;
    private PlayerDash _playerDash;
    private Health _playerHealth;

    private void Start()
    {
        _playerHealth.Death += GameManager_Death;
    }

    private void GameManager_Death(GameObject playerObject)
    {
        Debug.Log("player has died");
    }
}
