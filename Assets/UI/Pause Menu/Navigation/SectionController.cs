using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    private List<SectionBase> _sections = new();
    private int _sectionIndex = 0;

    private GameObject _holderObject;

    public void Init()
    {
        SectionBase[] sections = GetComponentsInChildren<SectionBase>();
        foreach (var section in sections) section.Init(this);

        _holderObject = transform.GetChild(0).gameObject;

        SwapSection();
    }

    public void RegisterSection(SectionBase section)
    {
        _sections.Add(section);
        Helpers.SortByStanceType(_sections);
        _sectionIndex = _sections.IndexOf(section);
        SwapSection();
    }

    public void SwapSection(int inputValue)
    {
        _sectionIndex = Helpers.GetIndexInBounds(_sectionIndex, inputValue, _sections.Count); SwapSection();
    }

    public void EnableHolder(bool enable)
    {
        _holderObject.SetActive(enable);
    }

    private void SwapSection()
    {
        if (_sections.Count > 0)
        {
            foreach (var section in _sections) section.Deactivate(); _sections[_sectionIndex].Activate();
        }
    }
}
