using System.Collections.Generic;
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

    private List<SingletonBase> _singletons = new();

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
        SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
        foreach (SingletonBase singleton in singletons)
        {
            _singletons.Add(singleton);
        }

        foreach (SingletonBase singleton in _singletons)
        {
            singleton.SingletonSetup();
        }

        _bloodwaveStats.Init();
        _playerStats.Init();

        _pauseMenuController.Init();
        _creatureManager.Init();
        _playerManager.Init();
        _audioSystem.Init();
    }

    private void Update()
    {
        _delta = Time.deltaTime;

        _creatureManager.Tick(_delta);
        _playerManager.Tick(_delta);
        _VFXManager.Tick(_delta);
    }

    private void LateUpdate()
    {
        _delta = Time.deltaTime;

        _creatureManager.LateTick(_delta);
        _playerManager.LateTick(_delta);
    }

    private void OnDisable()
    {
        _pauseMenuController.Cleanup();
        _playerManager.Cleanup();
    }
}