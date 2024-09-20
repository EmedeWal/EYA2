using System.Collections.Generic;
using UnityEngine;

public class Content : MonoBehaviour
{
    private PlayerInputHandler _playerInputHandler;

    //public IContentSection[] _contentSections;
    public IContentSection[] _contentSections;
    public int _sectionIndex = 0;

    public virtual void Init()
    {
        _playerInputHandler = PlayerInputHandler.Instance;
        _playerInputHandler.SwapSectionInputPerformed += Content_SwapContentInputPerformed;

        _contentSections = GetComponentsInChildren<IContentSection>();
        
        SwapSection();
    }

    protected virtual void Content_SwapContentInputPerformed(int inputValue)
    {
        _sectionIndex = Helpers.GetIndexInBounds(_sectionIndex, inputValue, _contentSections.Length);
        SwapSection();
    }

    private void SwapSection()
    {
        if (_contentSections.Length == 0) return;

        foreach (var section in _contentSections) section.Deselect();
        _contentSections[_sectionIndex].Select();
    }
}
