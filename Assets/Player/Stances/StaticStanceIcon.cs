using UnityEngine;

public class StaticStanceIcon : StanceIcon
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;

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
