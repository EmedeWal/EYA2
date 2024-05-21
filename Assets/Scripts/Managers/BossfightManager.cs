using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BossfightManager : MonoBehaviour
{
    [SerializeField] private UnityEvent playerHasWon;
    [SerializeField] private GameObject player;
    [SerializeField] private float victoryDelay;

    private int counter = 0;

    private void OnEnable()
    {
        Death.onEnemyDeath += CheckWinCondition;
    }

    private void OnDisable()
    {
        Death.onEnemyDeath -= CheckWinCondition;
    }

    public void CheckWinCondition(int something)
    {
        counter++;

        if (counter == 2)
        {
            playerHasWon.Invoke();
            player.GetComponent<PlayerInput>().enabled = false;
            Invoke(nameof(LoadMainMenu), victoryDelay);
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("Credits");
    }
}
