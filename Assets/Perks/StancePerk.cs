using UnityEngine;

public class StancePerk : Perk, IStanceDataProvider
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;
    private StanceIcon _stanceIcon;

    public StanceData StanceData => _stanceData;

    public override void Init()
    {
        base.Init();
        _stanceIcon = GetComponent<StanceIcon>();
        _stanceIcon.Icon.sprite = _stanceData.IconSprite;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _stanceIcon.Background.color = _stanceData.Color;
    }

    public override void OnExit()
    {
        base.OnExit();
        _stanceIcon.Background.color = Helpers.GetTransparentColor();
    }

    public override void OnClick()
    {
        base.OnClick();
    }
}
