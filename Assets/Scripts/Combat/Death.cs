using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public delegate void OnEnemyDeath(int amount);
    public static event OnEnemyDeath onEnemyDeath;

    [Header("GENERAL")]
    [SerializeField] private Animator animator;
    [SerializeField] private float deathDelay = 3f;

    public void HandleDeath()
    {
        if (animator != null) animator.SetTrigger("Death");
        Invoke(nameof(DestroyGameObject), deathDelay);
    }

    private void DestroyGameObject()
    {
        if (gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        onEnemyDeath?.Invoke(GetComponent<Enemy>().cost * 10);
        Destroy(gameObject);
    }
}
