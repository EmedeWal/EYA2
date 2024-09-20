using UnityEngine.UI;
using UnityEngine;

public class Header : MonoBehaviour
{
    [Header("SECTION CONTROLLER")]
    [SerializeField] private SectionController _sectionController;

    private Image _image;

    public void Init()
    {   
        _sectionController.Init();
        _image = GetComponentInChildren<Image>();
    }

    public void Select(Color color)
    {
        _sectionController.EnableSections(true);
        _image.color = color;
    }

    public void Deselect(Color color)
    {
        _sectionController.EnableSections(false);
        _image.color = color;
    }

    public void SwapSection(int inputValue)
    {
        _sectionController.SwapSection(inputValue);
    }
}
