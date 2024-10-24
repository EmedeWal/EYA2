using UnityEngine;

public class GameManager : SingletonBase
{
    #region Singleton
    public static GameManager Instance;

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private GameState _gameState = GameState.Starting;

    [Header("SCRIPTABLE OBJECTS INIT")]
    [SerializeField] private BloodwaveStats _bloodwaveStats;
    [SerializeField] private PlayerStats _playerStats;

    [Header("INIT CALLS")]
    [SerializeField] private PauseMenuController _pauseMenuController;
    [SerializeField] private CreatureManager _creatureManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private AudioSystem _audioSystem;

    [Header("TICK CALLS")]
    [SerializeField] private VFXManager _VFXManager;

    private float _delta;

    private void Awake()
    {
        if (_gameState == GameState.Starting)
        {
            Init();
        }
    }

    private void Update()
    {
        if (_gameState == GameState.Running)
        {
            _delta = Time.deltaTime;

            _creatureManager.Tick(_delta);
            _playerManager.Tick(_delta);
            _VFXManager.Tick(_delta);
        }
    }

    private void LateUpdate()
    {
        if (_gameState == GameState.Running)
        {
            _delta = Time.deltaTime;

            _creatureManager.LateTick(_delta);
            _playerManager.LateTick(_delta);
        }
    }

    private void OnDisable()
    {
        if (_gameState == GameState.Running)
        {
            Cleanup();
        }
    }

    private void Init()
    {
        SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
        foreach (SingletonBase singleton in singletons) singleton.SingletonSetup();

        _bloodwaveStats.Init();
        _playerStats.Init();

        _pauseMenuController.Init();
        _creatureManager.Init();
        _playerManager.Init();
        _audioSystem.Init();

        PlayerManager.PlayerDeath += GameManager_PlayerDeath;

        _gameState = GameState.Running; 
    }

    private void Cleanup()
    {
        _gameState = GameState.Ending;

        PlayerManager.PlayerDeath -= GameManager_PlayerDeath;

        _pauseMenuController.Cleanup();
        _creatureManager.Cleanup();
        _playerManager.Cleanup();
        _VFXManager.Cleanup();
    }

    private void GameManager_DeathAnimationFinished(AnimatorManager animatorManager)
    {
        animatorManager.DeathAnimationFinished -= GameManager_DeathAnimationFinished;

        Destroy(animatorManager);

        Debug.Log("Death animation finished. Player died. Not implemented");
    }

    private void GameManager_PlayerDeath(AnimatorManager animatorManager)
    {
        animatorManager.DeathAnimationFinished += GameManager_DeathAnimationFinished;

        Cleanup();
    }
}

public enum GameState
{
    Starting,
    Running,
    Ending
}