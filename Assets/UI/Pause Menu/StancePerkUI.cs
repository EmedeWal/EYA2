using UnityEngine;

public class StancePerkUI : MonoBehaviour, IClickable, IStanceDataProvider
{
    [Header("STANCE DATA REFERENCE")]
    [SerializeField] private StanceData _stanceData;
    private StanceIcon _stanceIcon;

    public StanceData StanceData => _stanceData;

    private void Awake()
    {
        _stanceIcon = GetComponent<StanceIcon>();
        _stanceIcon.Icon.sprite = _stanceData.IconSprite;
    }

    public void OnEnter()
    {
        Debug.Log("OnHover");

        _stanceIcon.Background.color = _stanceData.Color;
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        _stanceIcon.Background.color = Helpers.GetTransparentColor();
    }

    public void OnClick()
    {
        Debug.Log("OnClick");
    }
}
