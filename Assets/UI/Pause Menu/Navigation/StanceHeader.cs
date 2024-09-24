using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class StanceHeader : HeaderBase
{
    [Header("HORIZONTAL LAYOUT GROUP")]
    [SerializeField] private GameObject _horizontalLayoutGroupObject;

    [Header("VERTICAL LAYOUT GROUP")]
    [SerializeField] private GameObject _verticalLayoutGroupObject;

    private List<SectionControllerBase> _sections = new();
    private List<StaticStanceIcon> _staticStanceIcons = new();
    private List<StanceType> _unlockedStances = new();

    public override void Init()
    {
        base.Init();

        _horizontalLayoutGroupObject.SetActive(false);
        _verticalLayoutGroupObject.SetActive(true);

        _sections.AddRange(GetComponentsInChildren<SectionControllerBase>()); 
        _staticStanceIcons.AddRange(_verticalLayoutGroupObject.GetComponentsInChildren<StaticStanceIcon>());

        PlayerStanceManager.Instance.StanceUnlocked += StanceHeader_StanceUnlocked;
    }

    public override void Select(Color color)
    {
        base.Select(color);
    }

    public override void Deselect(Color color)
    {
        base.Deselect(color);
    }

    protected override void SwapSection()
    {
        base.SwapSection();

        foreach (var stanceIcon in _staticStanceIcons) stanceIcon.SetTransparentColor();
        _staticStanceIcons[_SectionIndex].SetStanceColor();
    }

    private void StanceHeader_StanceUnlocked(StanceType stanceType)
    {
        _horizontalLayoutGroupObject.SetActive(true);

        _unlockedStances.Add(stanceType);
        _unlockedStances = _unlockedStances.OrderBy(stance => stance.ToString()).ToList();

        int index = _unlockedStances.IndexOf(stanceType);
        _Sections.Add(_sections[index]);
        _SectionIndex = index;

        _staticStanceIcons[_SectionIndex].SetStanceIcon();

        SwapSection();
    }
}
