using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour 
{
    private void OnEnable()
    {
        ButtonUI.SetSelectedButton += EventManager_SetSelectedButton;
    }

    private void OnDisable()
    {
        ButtonUI.SetSelectedButton -= EventManager_SetSelectedButton;
    }

    private void EventManager_SetSelectedButton(GameObject buttonObject)
    {
        EventSystem.current.SetSelectedGameObject(buttonObject);
    }
}

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

[System.Serializable]
public class StatusEffectEvent : UnityEvent<int, bool> { }
