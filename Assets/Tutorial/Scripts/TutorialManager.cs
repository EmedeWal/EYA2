using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// TUTORIAL FLOW:
/// 1: Player moves towards lava.
/// 2: Player dashes over lava.
/// 3: Player destroys barrels by attacking.
/// 4: Player kills the first skeleton.
/// 5: Player unlocks the health potion.
/// 6: Player unlocks the teleporter upon finishing the heal.
/// 7: Update message to notify player of potion refilling.
/// 8: Spawn additional enemies upon descending the stairs.
/// 9: Unlock a stance shop which the player can use to unlock the first stance.
/// 10: Explain stance logic.
/// 11: Unlock mana potion.
/// 12: Upon finishing the mana potion, spawn additional enemies and notify the player of his ultimate.
/// 13: Unlock the other stance shops.
/// 14: Once the player has all stances, show him how to check which stances he has in the stance view. MAKE SURE TO UPDATE THE GHOST ULT BTW
/// 15: Then, show him the exit menu??? Not sure if that is the step.
/// 16: Infinite spawning to experiment. 
/// </summary>

public class TutorialManager : MonoBehaviour
{
    [Header("PLAYER REFERENCE")]
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private Resource[] _toInitializeArray;
    [SerializeField] private GameObject[] _toDisableArray;

    [Header("MESSAGES")]
    [SerializeField] private GameObject[] _messages;
    private int _messageIndex = 0;

    [Header("GENERAL EVENTS")]
    [SerializeField] private UnityEvent HealthPotionConsumed;
    [SerializeField] private UnityEvent UnlockedFirstStance;
    [SerializeField] private UnityEvent UnlockManaPotion;
    [SerializeField] private UnityEvent SpawnEnemies;
    [SerializeField] private UnityEvent ActivateSpawner;

    [Header("ENEMY EVENTS")]
    [SerializeField] private UnityEvent FirstBatchDefeated;
    [SerializeField] private UnityEvent SecondBatchDefeated;
    [SerializeField] private UnityEvent ThirdBatchDefeated;

    // Other
    private int _enemiesDied = 0;
    private int _stancesUnlocked = 0;

    private void Start()
    {
        UpdateMessage();

        _playerObject.GetComponent<Mana>().Init();

        foreach (Resource resource in _toInitializeArray)
        {
            resource.Init();
        }

        foreach (GameObject gameObject in _toDisableArray)
        {
            gameObject.SetActive(false);
        }

        _toDisableArray = null;
    }

    private void OnEnable()
    {
        Teleporter.TeleportedPlayer += TutorialManager_TeleportedPlayer;
        EnemyManager.EnemyDeath += TutorialManager_EnemyDeath;
        StancePurchaseMenu.UnlockStance += TutorialManager_UnlockStance;
    }

    private void OnDisable()
    {
        Teleporter.TeleportedPlayer -= TutorialManager_TeleportedPlayer;
        EnemyManager.EnemyDeath -= TutorialManager_EnemyDeath;
        StancePurchaseMenu.UnlockStance -= TutorialManager_UnlockStance;
        PlayerPause.Pause -= TutorialManager_Pause;
        //PlayerStanceManager.StanceSwappedDelegate -= TutorialManager_StanceSwap;
    }

    private void TutorialManager_PlayerHealed()
    {
        IncreaseIndex();
        OnHealthPotionConsumed();
        _playerObject.GetComponent<Health>().CoroutineCompleted -= TutorialManager_PlayerHealed;
    }

    private void TutorialManager_TeleportedPlayer()
    {
        IncreaseIndex();

        Teleporter.TeleportedPlayer -= TutorialManager_TeleportedPlayer;
    }

    private void TutorialManager_UnlockStance(StanceType stanceType)
    {
        _stancesUnlocked++;

        if (_stancesUnlocked == 1)
        {
            IncreaseIndex();
            OnUnlockedFirstStance();
            Invoke(nameof(OnUnlockManaPotion), 8f);
        }
        else if (_stancesUnlocked == 3)
        {
            IncreaseIndex();
            StancePurchaseMenu.UnlockStance -= TutorialManager_UnlockStance;
            //PlayerStanceManager.StanceSwappedDelegate += TutorialManager_StanceSwap;
        }
    }

    private void TutorialManager_ManaRestored()
    {
        IncreaseIndex();
        OnSpawnEnemies();
        _playerObject.GetComponent<Mana>().CoroutineCompleted -= TutorialManager_ManaRestored;
    }

    private void TutorialManager_StanceSwap()
    {
        IncreaseIndex();
        //PlayerStanceManager.StanceSwappedDelegate -= TutorialManager_StanceSwap;
        PlayerPause.Pause += TutorialManager_Pause;
    }

    private void TutorialManager_Pause()
    {
        IncreaseIndex();

        Invoke(nameof(CloseMessages), 6f);
        Invoke(nameof(OnActivateSpawner), 6f);

        PlayerPause.Pause -= TutorialManager_Pause;
    }

    private void TutorialManager_EnemyDeath()
    {
        _enemiesDied++;

        if (_enemiesDied == 1)
        {
            IncreaseIndex();
            OnFirstBatchDefeated();
        }
        else if (_enemiesDied == 3)
        {
            IncreaseIndex();
            OnSecondBatchDefeated();
        }
        else if (_enemiesDied == 7)
        {
            IncreaseIndex();
            OnThirdBatchDefeated();
            EnemyManager.EnemyDeath -= TutorialManager_EnemyDeath;
        }
    }

    private void OnHealthPotionConsumed()
    {
        HealthPotionConsumed.Invoke();
    }

    private void OnUnlockedFirstStance()
    {
        UnlockedFirstStance.Invoke();
    }

    private void OnUnlockManaPotion()
    {
        IncreaseIndex();
        UnlockManaPotion.Invoke();
        _playerObject.GetComponent<Mana>().CoroutineCompleted += TutorialManager_ManaRestored;
    }

    private void OnSpawnEnemies()
    {
        SpawnEnemies.Invoke();
    }

    private void OnActivateSpawner()
    {
        ActivateSpawner.Invoke();   
    }

    private void OnFirstBatchDefeated()
    {
        _playerObject.GetComponent<Health>().CoroutineCompleted += TutorialManager_PlayerHealed;
        FirstBatchDefeated.Invoke();
    }

    private void OnSecondBatchDefeated()
    {
        SecondBatchDefeated.Invoke();
    }

    private void OnThirdBatchDefeated()
    {
        ThirdBatchDefeated.Invoke();
    }

    public void IncreaseIndex()
    {
        _messageIndex++;
        UpdateMessage();
    }

    private void UpdateMessage()
    {
        CloseMessages();
        _messages[_messageIndex].SetActive(true);
    }

    private void CloseMessages()
    {
        foreach (var message in _messages) message.SetActive(false);
    }
}
