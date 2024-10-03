using System.Collections.Generic;
using UnityEngine;

public class StatSectionController : SectionControllerBase
{
    [Header("PLAYER STAT REFERENCE")]
    [SerializeField] private PlayerStats _playerStats;

    [Header("UI HOLDERS")]
    [SerializeField] private GameObject _leftHolder;
    [SerializeField] private GameObject _rightHolder;

    private List<StatUI> _statUIList = new();

    public override void Init(AudioDataUI audioDataUI)
    {
        base.Init(audioDataUI);

        _playerStats.StatChanged += OnStatChanged;

        _statUIList.AddRange(_leftHolder.GetComponentsInChildren<StatUI>());
        _statUIList.AddRange(_rightHolder.GetComponentsInChildren<StatUI>());

        foreach (var statUI in _statUIList)
        {
            statUI.Init();
            statUI.UpdateUI(_playerStats.GetCurrentStat(statUI.Stat));
        }
    }

    private void OnStatChanged(Stat stat, float value)
    {
        StatUI statUI = _statUIList.Find(s => s.Stat == stat);
        if (statUI != null)
        {
            statUI.UpdateUI(value);
        }
    }
}
