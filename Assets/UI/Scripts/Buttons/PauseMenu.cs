using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : ButtonUI
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _holder;
    [SerializeField] private GameObject _firstSelected;

    [Header("HEADERS")]
    [SerializeField] private Image[] _headers;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;

    [Header("MENUS")]
    [SerializeField] private GameObject[] _menus;
    [SerializeField] private int _menuIndex = 1;

    private PlayerInputHandler _playerInputHandler;
    private TimeSystem _timeSystem;

    public void Retry()
    {
        _timeSystem.ResetTimeScale();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        _timeSystem.ResetTimeScale();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitToDesktop()
    {
        _timeSystem.ResetTimeScale();
        Application.Quit();
    }

    public void Init()
    {
        _playerInputHandler = PlayerInputHandler.Instance;
        _timeSystem = TimeSystem.Instance;

        _playerInputHandler.PauseInputPerformed += PauseMenu_PauseInputPerformed;
        _holder.SetActive(false);
        SwapMenu();
    }

    private void PauseMenu_PauseInputPerformed()
    {
        if (_holder.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseMenu_SwapMenuInputPerformed(int inputValue)
    {
        _menuIndex = Helpers.GetIndexInBounds(_menuIndex, inputValue, _menus.Length); SwapMenu();
    }


    private void ResumeGame()
    { 
        _playerInputHandler.SwapMenuInputPerformed -= PauseMenu_SwapMenuInputPerformed;
        _playerInputHandler.ListenToCombatActions(true);
        _timeSystem.RevertToPreviousTimeScale();
        _holder.SetActive(false);
    }

    private void PauseGame()
    {
        _playerInputHandler.SwapMenuInputPerformed += PauseMenu_SwapMenuInputPerformed;
        _playerInputHandler.ListenToCombatActions(false);
        _timeSystem.SetTimeScale(0);
        _holder.SetActive(true);

        OnSetSelectedButton(_firstSelected);
    }

    private void SwapMenu()
    {
        foreach (var menu in _menus) menu.SetActive(false);
        _menus[_menuIndex].SetActive(true);

        foreach (var header in _headers) header.color = _defaultColor;
        _headers[_menuIndex].color = _selectedColor;
    }
}
