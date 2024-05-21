using UnityEngine;

public abstract class ButtonUI : MonoBehaviour
{
    public delegate void ButtonUI_SetSelectedButton(GameObject button);
    public static event ButtonUI_SetSelectedButton SetSelectedButton;

    protected void OnSetSelectedButton(GameObject button)
    {
        SetSelectedButton?.Invoke(button);
    }
}
