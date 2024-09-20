using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    private List<Section> _sections = new();
    private int _sectionIndex = 0;

    public void Init()
    {
        Section[] sections = GetComponentsInChildren<Section>();
        foreach (var section in sections) { _sections.Add(section); section.Init(); }

        SwapSection();
    }

    public void SwapSection(int inputValue)
    {
        _sectionIndex = Helpers.GetIndexInBounds(_sectionIndex, inputValue, _sections.Count); SwapSection();
    }

    public void EnableSections(bool enable)
    {
        foreach (var section in _sections)
        {
            if (enable)
            {
                section.Activate();
            }
            else
            {       
                section.Deactivate();
            }
        }
    }

    private void SwapSection()
    {
        foreach (var section in _sections) section.Deactivate();
        _sections[_sectionIndex].Activate();
    }
}
