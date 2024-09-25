using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StanceHeader : HeaderBase
{
    [Header("LAYOUT GROUP")]
    [SerializeField] private GameObject _horizontalLayoutGroupObject;

    private List<SectionControllerBase> _sectionControllers = new();

    public override void Init()
    {
        base.Init();

        _horizontalLayoutGroupObject.SetActive(false);
        _sectionControllers.AddRange(GetComponentsInChildren<SectionControllerBase>());
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

    private void StanceHeader_StanceUnlocked(StanceType stanceType)
    {
        _horizontalLayoutGroupObject.SetActive(true);

        int index = 0;

        switch (stanceType)
        {
            case StanceType.Ghost:
                index = 0;
                break;

            case StanceType.Orc:
                index = 1;
                break;

            case StanceType.Vampire:
                index = 2;
                break;
        }

        SectionControllerBase sectionController = _sectionControllers[index];

        AddSectionController(sectionController);
        _SectionsControllers = _SectionsControllers.OrderBy(section => section.ToString()).ToList();
        _SectionIndex = _SectionsControllers.IndexOf(sectionController);
        SwapSection();
    }
}