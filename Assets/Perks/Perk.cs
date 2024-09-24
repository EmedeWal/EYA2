using UnityEngine;

public class Perk : MonoBehaviour, IClickable
{
    public PerkData _perkData;

    private PerkScreen _perkScreen;

    public virtual void Init()
    {
        _perkScreen = PerkScreen.Instance;
    }

    public virtual void OnEnter()
    {
        _perkScreen.SetText(_perkData.Title, _perkData.Description);
    }

    public virtual void OnExit()
    {
        _perkScreen.SetText("", "");
    }

    public virtual void OnClick()
    {

    }
}
