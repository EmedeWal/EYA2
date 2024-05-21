using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
