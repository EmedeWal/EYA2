using System.Collections.Generic;
using UnityEngine;

public class StanceWheelController : MonoBehaviour
{
    #region Singleton
    public static StanceWheelController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        StanceSlot[] StanceSlots = GetComponentsInChildren<StanceSlot>();
        foreach (var slot in StanceSlots) _stanceSlots.Add(slot);

        _playerInputHandler = FindObjectOfType<PlayerInputHandler>();
        _canvasObject = transform.GetChild(0).gameObject;
        _canvasObject.SetActive(false);
    }
    #endregion

    public bool _StanceWheelActive { get; private set; }

    private List<StanceSlot> _stanceSlots = new();
    private PlayerInputHandler _playerInputHandler;
    private GameObject _canvasObject;
    private int _currentIndex = 0;

    private void OnEnable()
    {
        _playerInputHandler.StanceWheelInput += SpellWheelController_SpellWheelInputPerformed;
    }

    private void OnDisable()
    {
        _playerInputHandler.StanceWheelInput -= SpellWheelController_SpellWheelInputPerformed;
    }

    private void SpellWheelController_RightJoystickInputPerformed()
    {
        float horizontal = _playerInputHandler._RightStickX;
        float vertical = _playerInputHandler._RightStickY;

        if (horizontal == 0 && vertical == 0) return;

        float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
        int newSelection = (int)Mathf.Round(angle / (360f / _stanceSlots.Count));
        newSelection = (newSelection + _stanceSlots.Count) % _stanceSlots.Count;

        if (newSelection != _currentIndex)
        {
            _currentIndex = newSelection;
            UpdateSelectedSpell();
        }
    }

    private void SpellWheelController_SpellWheelInputPerformed()
    {
        CameraManager.Instance.RotationEnabled = false;
        TimeManager.Instance.SetTimeScale(0.25f);

        _playerInputHandler.RightJoystickInputPerformed += SpellWheelController_RightJoystickInputPerformed;
        _canvasObject.SetActive(true);
        _StanceWheelActive = true;
    }

    private void SpellWheelController_SpellWheelInputCanceled()
    {
        CameraManager.Instance.RotationEnabled = true;
        TimeManager.Instance.ResetTimeScale();

        _playerInputHandler.RightJoystickInputPerformed -= SpellWheelController_RightJoystickInputPerformed;
        _canvasObject.SetActive(false);
        _StanceWheelActive = false;
    }

    private void UpdateSelectedSpell()
    {
        //foreach (SpellSlot slot in _stanceSlots) slot.Hover(false); _stanceSlots[_currentIndex].Hover(true);
        //_playerDataManager.SetSelectedSpell(_stanceSlots[_currentIndex].Spell);
    }
}
