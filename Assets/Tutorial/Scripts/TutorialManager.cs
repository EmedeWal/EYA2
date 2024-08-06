using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
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

    private void Awake()
    {
        UpdateMessage();
    }

    private void OnEnable()
    {
        //PlayerHealth.PlayerHealed += TutorialManager_PlayerHealed;
        Teleporter.TeleportedPlayer += TutorialManager_TeleportedPlayer;
        EnemyHealth.EnemyDied += TutorialManager_EnemyHasDied;
        StancePurchaseMenu.UnlockStance += TutorialManager_UnlockStance;
        //PlayerMana.ManaRestored += TutorialManager_ManaRestored;
    }

    private void OnDisable()
    {
        //PlayerHealth.PlayerHealed -= TutorialManager_PlayerHealed;
        Teleporter.TeleportedPlayer -= TutorialManager_TeleportedPlayer;
        EnemyHealth.EnemyDied -= TutorialManager_EnemyHasDied;
        StancePurchaseMenu.UnlockStance -= TutorialManager_UnlockStance;
        //PlayerMana.ManaRestored -= TutorialManager_ManaRestored;
        PlayerPause.Pause -= TutorialManager_Pause;
        PlayerStanceManager.StanceSwap -= TutorialManager_StanceSwap;
    }

    private void TutorialManager_PlayerHealed()
    {
        IncreaseIndex();

        //PlayerHealth.PlayerHealed -= TutorialManager_PlayerHealed;

        OnHealthPotionConsumed();
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
            OnUnlockedFirstStance();

            IncreaseIndex();
            Invoke(nameof(OnUnlockManaPotion), 8f);
        }
        else if (_stancesUnlocked == 3)
        {
            IncreaseIndex();

            StancePurchaseMenu.UnlockStance -= TutorialManager_UnlockStance;

            PlayerStanceManager.StanceSwap += TutorialManager_StanceSwap;
        }
    }

    private void TutorialManager_ManaRestored()
    {
        IncreaseIndex();

        //PlayerMana.ManaRestored -= TutorialManager_ManaRestored;

        OnSpawnEnemies();
    }

    private void TutorialManager_StanceSwap()
    {
        IncreaseIndex();

        PlayerStanceManager.StanceSwap -= TutorialManager_StanceSwap;

        PlayerPause.Pause += TutorialManager_Pause;
    }

    private void TutorialManager_Pause()
    {
        IncreaseIndex();

        Invoke(nameof(CloseMessages), 6f);
        Invoke(nameof(OnActivateSpawner), 6f);

        PlayerPause.Pause -= TutorialManager_Pause;
    }

    private void TutorialManager_EnemyHasDied()
    {
        _enemiesDied++;

        if (_enemiesDied == 1)
        {
            IncreaseIndex();
            FirstBatchDefeated.Invoke();
        }
        else if (_enemiesDied == 3)
        {
            IncreaseIndex();
            SecondBatchDefeated.Invoke();
        }
        else if (_enemiesDied == 7)
        {
            IncreaseIndex();
            OnThirdBatchDefeated();

            EnemyHealth.EnemyDied -= TutorialManager_EnemyHasDied;
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
    }

    private void OnSpawnEnemies()
    {
        SpawnEnemies.Invoke();
    }

    private void OnActivateSpawner()
    {
        ActivateSpawner.Invoke();   
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
