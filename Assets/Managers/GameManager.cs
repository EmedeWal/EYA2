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
    [SerializeField] private PlayerStats _playerStats;

    [Header("INIT CALLS")]
    [SerializeField] private PauseMenuController _pauseMenuController;
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
            if (singleton != this)
            {
                _singletons.Add(singleton);
            }
        }

        foreach (SingletonBase singleton in _singletons)
        {
            singleton.SingletonSetup();
        }

        _playerStats.Init();

        _pauseMenuController.Init();
        _playerManager.Init();
        _audioSystem.Init();
    }

    private void Update()
    {
        _delta = Time.deltaTime;

        _playerManager.Tick(_delta);
        _VFXManager.Tick(_delta);
    }

    private void LateUpdate()
    {
        _delta = Time.deltaTime;

        _playerManager.LateTick(_delta);
    }

    private void OnDisable()
    {
        _pauseMenuController.Cleanup();
        _playerManager.Cleanup();
    }
}
