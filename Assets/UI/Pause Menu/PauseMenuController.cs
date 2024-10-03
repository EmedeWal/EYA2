using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class PauseMenuController : MonoBehaviour
{
    [Header("AUDIO DATA UI REFERENCE")]
    [SerializeField] private AudioDataUI _audio;

    [Header("VARIABLES")]
    [SerializeField] private float _pauseToggleCooldown = 1;

    [Header("HEADERS")]
    [SerializeField] private GameObject _headerHolderObject;
    [SerializeField] private Color _deselectedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private int _headerIndex = 2;
    private List<Header> _headers = new();

    [Header("CURSOR")]
    [SerializeField] private Image _cursorImage; 
    [SerializeField] private float _cursorSpeed = 5f;
    private Vector3 _cursorPosition = new(Screen.width / 2, Screen.height / 2, 0);

    private PlayerInputHandler _playerInputHandler;
    private TimeSystem _timeSystem;

    private GameObject _holder;
    private LayerMask _clickable;
    private IClickable _currentClickable;

    private SoulsUI _soulsUI;

    public void Init()
    {
        _holder = transform.GetChild(0).gameObject;

        _audio.System = AudioSystem.Instance;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        _audio.UncommonSource = audioSources[0];
        _audio.CommonSource = audioSources[1];
        _audio.FailedSource = audioSources[2];

        _headers.AddRange(_headerHolderObject.GetComponentsInChildren<Header>());
        foreach (var header in _headers) header.Init(_audio);

        _soulsUI = GetComponentInChildren<SoulsUI>(); 
        _soulsUI.Init();

        _playerInputHandler = PlayerInputHandler.Instance;
        _timeSystem = TimeSystem.Instance;

        PerkScreen.Instance.Init();

        _playerInputHandler.PauseInputPerformed += PauseMenu_PauseInputPerformed;
        _holder.SetActive(false);

        _clickable = LayerMask.GetMask("Clickable");

        _cursorImage.gameObject.SetActive(true);

        SwapHeader();
    }

    public void Cleanup()
    {
        _playerInputHandler.BackInputPerformed -= PauseMenu_BackInputPerformed;
        _playerInputHandler.ClickInputPerformed -= PauseMenu_ClickInputPerformed;
        _playerInputHandler.PauseInputPerformed -= PauseMenu_PauseInputPerformed;
        _playerInputHandler.SwapHeaderInputPerformed -= PauseMenu_SwapHeaderInputPerformed;
        _playerInputHandler.SwapSectionInputPerformed -= PauseMenu_SwapSectionInputPerformed;
    }

    private void PauseMenu_BackInputPerformed()
    {
        ResumeGame();
    }

    private void PauseMenu_ClickInputPerformed()
    {
        _currentClickable?.OnClick();
    }

    private void PauseMenu_PauseInputPerformed()
    {
        _playerInputHandler.PauseInputPerformed -= PauseMenu_PauseInputPerformed;

        if (_holder.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }

        StartCoroutine(ResubscribeToPauseInputPerformed());
    }

    private void PauseMenu_SwapHeaderInputPerformed(int inputValue)
    {
        _audio.System.PlayAudioClip(_audio.CommonSource, _audio.SwapClip, _audio.SwapVolume, _audio.SwapOffset);
        _headerIndex = Helpers.GetIndexInBounds(_headerIndex, inputValue, _headers.Count); 
        SwapHeader();
    }

    private void PauseMenu_SwapSectionInputPerformed(int inputValue)
    {
        _audio.System.PlayAudioClip(_audio.CommonSource, _audio.SwapClip, _audio.SwapVolume, _audio.SwapOffset);
        _headers[_headerIndex].SwapSection(inputValue);
    }

    private void ResumeGame()
    {
        _audio.System.PlayAudioClip(_audio.UncommonSource, _audio.ResumeClip, _audio.ResumeVolume, _audio.ResumeOffset);

        _playerInputHandler.BackInputPerformed -= PauseMenu_BackInputPerformed;
        _playerInputHandler.ClickInputPerformed -= PauseMenu_ClickInputPerformed;
        _playerInputHandler.SwapHeaderInputPerformed -= PauseMenu_SwapHeaderInputPerformed;
        _playerInputHandler.SwapSectionInputPerformed -= PauseMenu_SwapSectionInputPerformed;
        _playerInputHandler.ListenToCombatActions(true);
        _timeSystem.RevertToPreviousTimeScale();
        _holder.SetActive(false);

        StopAllCoroutines();
    }

    private void PauseGame()
    {
        _audio.System.PlayAudioClip(_audio.UncommonSource, _audio.PauseClip, _audio.PauseVolume, _audio.PauseOffset);

        _playerInputHandler.BackInputPerformed += PauseMenu_BackInputPerformed;
        _playerInputHandler.ClickInputPerformed += PauseMenu_ClickInputPerformed;
        _playerInputHandler.SwapHeaderInputPerformed += PauseMenu_SwapHeaderInputPerformed;
        _playerInputHandler.SwapSectionInputPerformed += PauseMenu_SwapSectionInputPerformed;
        _playerInputHandler.ListenToCombatActions(false);
        _timeSystem.SetTimeScale(0);
        _holder.SetActive(true);

        StartCoroutine(HandleCursorCoroutine());
    }

    private void SwapHeader()
    {
        foreach (var header in _headers) header.Deselect(_deselectedColor);
        _headers[_headerIndex].Select(_selectedColor);
    }

    private IEnumerator ResubscribeToPauseInputPerformed()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _pauseToggleCooldown)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _playerInputHandler.PauseInputPerformed += PauseMenu_PauseInputPerformed;
    }

    private IEnumerator HandleCursorCoroutine()
    {
        while (true)
        {
            float horizontalInput = _playerInputHandler._LeftStickX;
            float verticalInput = _playerInputHandler._LeftStickY;

            _cursorPosition += _cursorSpeed * Time.unscaledDeltaTime * 100 * new Vector3(horizontalInput, verticalInput, 0);

            _cursorPosition.x = Mathf.Clamp(_cursorPosition.x, 0, Screen.width);
            _cursorPosition.y = Mathf.Clamp(_cursorPosition.y, 0, Screen.height);

            _cursorImage.transform.position = _cursorPosition;

            if (TryGetRayCast(_cursorPosition, _clickable, out GameObject hitObject))
            {
                if (hitObject.TryGetComponent(out IClickable clickable))
                {
                    if (_currentClickable != clickable)
                    {
                        clickable.OnEnter();
                        _currentClickable?.OnExit();
                        _currentClickable = clickable;
                    }
                }
            }
            else
            {
                _currentClickable?.OnExit();
                _currentClickable = null;
            }

            yield return null;
        }
    }

    public bool TryGetRayCast(Vector3 cursorPosition, LayerMask layer, out GameObject hitObject)
    {
        EventSystem eventSystem = EventSystem.current;
        var eventData = new PointerEventData(eventSystem) { position = cursorPosition };
        var results = new List<RaycastResult>();

        eventSystem.RaycastAll(eventData, results);
        var uiResult = results.FirstOrDefault(r => (layer.value & (1 << r.gameObject.layer)) != 0);

        if (uiResult.gameObject != null)
        {
            hitObject = uiResult.gameObject; return true;
        }

        hitObject = null; return false;
    }
}
