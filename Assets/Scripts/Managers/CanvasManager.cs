using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public void SwapCanvasState()
    {
        if (canvas != null) Debug.Log(canvas.name + " is the enemy canvas");
        if (canvas == null) return;

        if (canvas.gameObject.activeSelf) canvas.gameObject.SetActive(false);
        else canvas.gameObject.SetActive(true);
    }
}
