using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour
{
    [Header("Credit Scrolling")]
    [SerializeField] private RectTransform textHolder;
    [SerializeField] private float duration;
    [SerializeField] private float distance;

    private float speed;

    [Header("Input Prompt")]
    [SerializeField] private GameObject inputPrompt;

    private void Start()
    {
        if (textHolder == null) return;
        speed = distance / duration;

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (textHolder != null)
        {
            textHolder.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            if (textHolder.anchoredPosition.y >= distance) LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}