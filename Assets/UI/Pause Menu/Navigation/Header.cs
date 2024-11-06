using System.Collections.Generic;
using EmeWillem.Utilities;
using UnityEngine.UI;
using UnityEngine;

public class Header : MonoBehaviour
{
    private GameObject _iconParent;
    private List<SectionIcon> _sectionIcons = new();

    private GameObject _holderObject;
    private Image _image;

    private List<SectionControllerBase> _sectionControllers = new();
    private int _sectionIndex = 0;

    public void Init(AudioDataUI audio)
    {
        _holderObject = transform.GetChild(2).gameObject;
        _iconParent = _holderObject.transform.GetChild(0).gameObject;

        _sectionIcons.AddRange(_iconParent.GetComponentsInChildren<SectionIcon>());
        foreach (var sectionIcon in _sectionIcons) sectionIcon.Init();

        _sectionControllers.AddRange(_holderObject.GetComponentsInChildren<SectionControllerBase>());
        foreach (var sectionController in _sectionControllers) sectionController.Init(audio);

        _image = GetComponentInChildren<Image>();
        _holderObject.SetActive(false);
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
            foreach (var icon in _sectionIcons) icon.Deselect(); _sectionIcons[_sectionIndex].Select();
        }
    }
}