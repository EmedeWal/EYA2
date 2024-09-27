using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class PauseMenuController : MonoBehaviour
{
    [Header("HEADERS")]
    [SerializeField] private GameObject _headerHolderObject;
    [SerializeField] private Color _deselectedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private int _headerIndex = 2;
    private List<HeaderBase> _headers = new();

    [Header("RESOURCES")]
    [SerializeField] private GameObject _resourceHolderObject;
    private SoulsUI _soulsUI;

    [Header("CURSOR")]
    [SerializeField] private Image _cursorImage; 
    [SerializeField] private float _cursorSpeed = 5f;
    private Vector3 _cursorPosition = new(Screen.width / 2, Screen.height / 2, 0);

    private PlayerInputHandler _playerInputHandler;
    private TimeSystem _timeSystem;
    private GameObject _holder;
    private LayerMask _clickable;

    private IClickable _currentClickable;

    public void Init()
    {
        _holder = transform.GetChild(0).gameObject;

        _headers.AddRange(_headerHolderObject.GetComponentsInChildren<HeaderBase>());
        foreach (var header in _headers) header.Init();

        _soulsUI = _resourceHolderObject.GetComponentInChildren<SoulsUI>();
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

    public void Tick()
    {
        _headers[_headerIndex].Tick();
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
