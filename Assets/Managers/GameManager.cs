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

    [Header("INIT CALLS")]
    [SerializeField] private PauseMenuController _pauseMenuController;
    [SerializeField] private PlayerManager _playerManager;

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

        _pauseMenuController.Init();
        _playerManager.Init();
    }

    private void Update()
    {
        _pauseMenuController.Tick();
    }
}
