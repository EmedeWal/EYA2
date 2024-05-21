using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : ButtonUI
{
    [SerializeField] private GameObject _holder;
    [SerializeField] private GameObject _firstSelected;

    [Header("HEADERS")]
    [SerializeField] private Image[] _headers;
    [SerializeField] private Color _selectedColor;

    [Header("MENUS")]
    [SerializeField] private GameObject[] _menus;
    [SerializeField] private int _menuIndex = 1;

    private void Start()
    {
        SwapMenu();
        ManageHolder(false);
    }

    private void OnEnable()
    {
        PlayerPause.Pause += PauseMenu_Pause;
        PlayerPause.Resume += PauseMenu_Resume;
        PlayerPause.SwapMenu += PauseMenu_SwapMenu;
    }

    private void OnDisable()
    {
        PlayerPause.Pause -= PauseMenu_Pause;
        PlayerPause.Resume -= PauseMenu_Resume;
        PlayerPause.SwapMenu -= PauseMenu_SwapMenu;
    }

    private void PauseMenu_Pause()
    {
        Time.timeScale = 0;
        OpenPauseMenu();
    }

    private void PauseMenu_Resume()
    {
        Time.timeScale = 1;
        ClosePauseMenu();
    }

    private void PauseMenu_SwapMenu(int inputValue)
    {
        int lastPosition = _menus.Length - 1;

        _menuIndex += inputValue;

        if (_menuIndex < 0) _menuIndex = lastPosition;
        else if (_menuIndex > lastPosition) _menuIndex = 0;

        SwapMenu();
    }

    private void OpenPauseMenu()
    {
        ManageHolder(true);
        OnSetSelectedButton(_firstSelected);
    }

    private void ClosePauseMenu()
    {
        ManageHolder(false);
    }

    private void SwapMenu()
    {
        foreach (var menu in _menus) menu.SetActive(false);
        _menus[_menuIndex].SetActive(true);

        SwapHeader();
    }

    private void SwapHeader()
    {
        foreach (var header in _headers) header.color = Color.white;
        _headers[_menuIndex].color = _selectedColor;
    }

    private void ManageHolder(bool active)
    {
        _holder.SetActive(active);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitToDesktop()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
