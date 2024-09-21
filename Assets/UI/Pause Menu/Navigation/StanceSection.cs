using UnityEngine;

public class StanceSection : SectionBase, IStanceDataProvider
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;

    private SectionController _sectionController;
    private StanceIcon _stanceIcon;

    public StanceData StanceData => _stanceData;

    public override void Init(SectionController controller)
    {
        base.Init(controller);
        _sectionController = controller;
        _stanceIcon = GetComponent<StanceIcon>();
        PlayerStanceManager.Instance.StanceUnlocked += OnStanceUnlocked;
    }

    public override void Activate()
    {
        base.Activate();
        _stanceIcon.Background.color = _stanceData.Color;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _stanceIcon.Background.color = Helpers.GetTransparentColor();
    }

    private void OnStanceUnlocked(StanceType unlockedStance)
    {
        if (_stanceData.StanceType == unlockedStance)
        {
            _sectionController.RegisterSection(this);
            _stanceIcon.Icon.sprite = _stanceData.IconSprite;
        }
    }
}
