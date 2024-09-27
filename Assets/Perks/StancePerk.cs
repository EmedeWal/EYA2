using UnityEngine;

public class StancePerk : Perk, IStanceDataProvider
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;

    [Header("SPRITE")]
    [SerializeField] private Sprite _sprite;

    private StanceIcon _stanceIcon;

    private Sprite _defaultSprite;

    public StanceData StanceData => _stanceData;

    public override void Init()
    {
        base.Init();
        _stanceIcon = GetComponent<StanceIcon>();
        _defaultSprite = _stanceIcon.Icon.sprite;
        _stanceIcon.Background.color = Helpers.GetTransparentColor();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Unlock()
    {
        base.Unlock();

        if (Locked) return;
        _stanceIcon.Icon.sprite = _sprite;
    }

    public override void Purchase()
    {
        base.Purchase();

        if (Purchased)
        {
            _stanceIcon.Background.color = _stanceData.Color;
        }
    }

    public override void LockBranch()
    {
        base.LockBranch();

        _stanceIcon.Icon.sprite = _defaultSprite;
    }
}