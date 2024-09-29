using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Header : MonoBehaviour
{
    private GameObject _holderObject;
    private Image _image;

    private List<SectionControllerBase> _sectionControllers = new();
    private int _sectionIndex = 0;

    public void Init()
    {
        _holderObject = transform.GetChild(2).gameObject;
        _holderObject.SetActive(false);

        _sectionControllers.AddRange(_holderObject.GetComponentsInChildren<SectionControllerBase>());
        foreach (var sectionController in _sectionControllers) sectionController.Init();

        _image = GetComponentInChildren<Image>();

        SwapSection(0);
    }

    public void Select(Color color)
    {
        _holderObject.SetActive(true);
        _image.color = color;
    }

    public void Deselect(Color color)
    {
        _holderObject.SetActive(false);
        _image.color = color;
    }

    public void SwapSection(int inputValue)
    {
        if (_sectionControllers.Count > 0)
        {
            _sectionIndex = Helpers.GetIndexInBounds(_sectionIndex, inputValue, _sectionControllers.Count);
            foreach (var section in _sectionControllers) section.Deselect(); _sectionControllers[_sectionIndex].Select();
        }
    }
}