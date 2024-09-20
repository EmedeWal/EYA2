using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    //[Header("REFERENCES")]
    //[SerializeField] private GameObject _firstSelected;

    [Header("HEADERS")]
    [SerializeField] private GameObject _headerHolderObject;
    [SerializeField] private Color _deselectedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private int _headerIndex = 2;
    private List<Header> _headers = new();

    private PlayerInputHandler _playerInputHandler;
    private TimeSystem _timeSystem;
    private GameObject _holder;

    //public void Retry()
    //{
    //    _timeSystem.ResetTimeScale();
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    //public void QuitToMainMenu()
    //{
    //    _timeSystem.ResetTimeScale();
    //    SceneManager.LoadScene("MainMenu");
    //}

    //public void QuitToDesktop()
    //{
    //    _timeSystem.ResetTimeScale();
    //    Application.Quit();
    //}

    public void Init()
    {
        _holder = transform.GetChild(0).gameObject;

        Header[] headers = _headerHolderObject.GetComponentsInChildren<Header>();
        foreach (var header in headers) { _headers.Add(header); header.Init(); }

        _playerInputHandler = PlayerInputHandler.Instance;
        _timeSystem = TimeSystem.Instance;

        _playerInputHandler.PauseInputPerformed += PauseMenu_PauseInputPerformed;
        _holder.SetActive(false);

        SwapHeader();
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

    private void PauseMenu_SwapHeaderInputPerformed(int inputValue)
    {
        _headerIndex = Helpers.GetIndexInBounds(_headerIndex, inputValue, _headers.Count); SwapHeader();
    }

    private void PauseMenu_SwapSectionInputPerformed(int inputValue)
    {
        _headers[_headerIndex].SwapSection(inputValue);
    }


    private void ResumeGame()
    { 
        _playerInputHandler.SwapHeaderInputPerformed -= PauseMenu_SwapHeaderInputPerformed;
        _playerInputHandler.SwapSectionInputPerformed += PauseMenu_SwapSectionInputPerformed;
        _playerInputHandler.ListenToCombatActions(true);
        _timeSystem.RevertToPreviousTimeScale();
        _holder.SetActive(false);
    }

    private void PauseGame()
    {
        _playerInputHandler.SwapHeaderInputPerformed += PauseMenu_SwapHeaderInputPerformed;
        _playerInputHandler.SwapSectionInputPerformed += PauseMenu_SwapSectionInputPerformed;
        _playerInputHandler.ListenToCombatActions(false);
        _timeSystem.SetTimeScale(0);
        _holder.SetActive(true);

        //OnSetSelectedButton(_firstSelected);
    }

    private void SwapHeader()
    {
        foreach (var header in _headers) header.Deselect(_deselectedColor);
        _headers[_headerIndex].Select(_selectedColor);
    }
}
