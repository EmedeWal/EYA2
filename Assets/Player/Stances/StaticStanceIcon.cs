using UnityEngine;

public class StaticStanceIcon : StanceIcon
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;

    [Header("UI REFERENCES")]
    [SerializeField] private GameObject _glow;
    [SerializeField] private GameObject _foredrop;

    public void Init()
    {
        _foredrop.SetActive(false);
        SetStanceIcon();
        SetStanceColor();
    }

    public void SetGlow(bool active)
    {
        _glow.SetActive(active);
    }

    public void SetStanceIcon()
    {
        Icon.sprite = _stanceData.IconSprite;
    }

    public void SetStanceColor()
    {
        Background.color = _stanceData.Color;
    }

    public void SetTransparentColor()
    {
        Background.color = Helpers.GetTransparentColor();
    }
}
