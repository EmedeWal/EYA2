using UnityEngine.UI;
using UnityEngine;
public class StanceColorSetter : MonoBehaviour
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;

    [Header("IMAGE REFERENCE")]
    [SerializeField] private Image _image;

    private void Awake()
    {
        _image.color = _stanceData.Color;
    }
}