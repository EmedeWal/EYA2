using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeaderBase : MonoBehaviour
{
    protected List<SectionControllerBase> _SectionsControllers = new();
    protected int _SectionIndex = 0;

    private GameObject _holderObject;
    private Image _image;

    public virtual void Init()
    {
        SectionControllerBase[] sections = GetComponentsInChildren<SectionControllerBase>();
        foreach (var section in sections) section.Init();

        _holderObject = transform.GetChild(2).gameObject;
        _image = GetComponentInChildren<Image>();
    }

    public virtual void Select(Color color)
    {
        _holderObject.SetActive(true);
        _image.color = color;
    }

    public virtual void Deselect(Color color)
    {
        _holderObject.SetActive(false);
        _image.color = color;
    }

    public void SwapSection(int inputValue)
    {
        _SectionIndex = Helpers.GetIndexInBounds(_SectionIndex, inputValue, _SectionsControllers.Count); SwapSection();
    }

    protected void AddSectionController(SectionControllerBase sectionController)
    {
        _SectionsControllers.Add(sectionController); sectionController.Added();
    }

    protected void SwapSection()
    {
        if (_SectionsControllers.Count > 0)
        {
            foreach (var section in _SectionsControllers) section.Deselect(); _SectionsControllers[_SectionIndex].Select();
        }
    }
}