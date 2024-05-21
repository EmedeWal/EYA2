using UnityEngine;

public class PlayerPause : MonoBehaviour
{
    private PlayerInputManager _inputManager;

    public delegate void PlayerPause_Pause();
    public static event PlayerPause_Pause Pause;

    public delegate void PlayerPause_Resume();
    public static event PlayerPause_Resume Resume;

    public delegate void PlayerPause_SwapMenu(int index);
    public static event PlayerPause_SwapMenu SwapMenu;

    private bool _paused = false;

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _inputManager.PauseInput_Performed += PlayerPause_PauseInput_Performed;
        _inputManager.SwapMenuInput_Performed += PlayerPause_SwapMenuInput_Performed;
    }

    private void OnDisable()
    {
        _inputManager.PauseInput_Performed -= PlayerPause_PauseInput_Performed;
        _inputManager.SwapMenuInput_Performed -= PlayerPause_SwapMenuInput_Performed;
    }

    private void PlayerPause_PauseInput_Performed()
    {
        if (_paused)
        {
            _paused = false;
            OnResume();
        }
        else
        {
            _paused = true;
            OnPause();
        }
    }

    private void PlayerPause_SwapMenuInput_Performed(float inputValue)
    {
        if (_paused)
        {
            int index = Mathf.FloorToInt(inputValue);

            SwapMenu?.Invoke(index);
        }
    }

    private void OnPause()
    {
        Pause?.Invoke();
    }

    private void OnResume()
    {
        Resume?.Invoke();   
    }
}
