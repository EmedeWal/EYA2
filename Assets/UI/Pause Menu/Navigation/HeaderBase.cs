using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeaderBase : MonoBehaviour
{
    protected List<SectionControllerBase> _Sections = new();
    protected int _SectionIndex = 0;

    private GameObject _holderObject;
    private Image _image;

    public virtual void Init()
    {
        SectionControllerBase[] sections = GetComponentsInChildren<SectionControllerBase>();
        foreach (var section in sections) section.Init();

        _holderObject = transform.GetChild(2).gameObject;
        _image = GetComponentInChildren<Image>();

        // Derivative classes decide what to fill sections with
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
        _SectionIndex = Helpers.GetIndexInBounds(_SectionIndex, inputValue, _Sections.Count); SwapSection();
    }

    protected virtual void SwapSection()
    {
        if (_Sections.Count > 0)
        {
            foreach (var section in _Sections) section.Deselect(); _Sections[_SectionIndex].Select();
        }
    }
}