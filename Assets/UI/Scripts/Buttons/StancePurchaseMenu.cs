using UnityEngine;

public class StancePurchaseMenu : ButtonUI
{
    public delegate void StancePurchaseMenu_UnlockStance(StanceType stanceType);
    public static event StancePurchaseMenu_UnlockStance UnlockStance;

    [Header("REFERENCES")]
    [SerializeField] private GameObject _holder;
    [SerializeField] private GameObject[] _stancePurchaseTemplates;

    private void Start()
    {
        _holder.SetActive(false);
    }

    private void OnEnable()
    {
        StanceShop.Interaction += StancePurchaseMenu_Interaction;
    }

    private void OnDisable()
    {
        StanceShop.Interaction -= StancePurchaseMenu_Interaction;
    }

    private void StancePurchaseMenu_Interaction()
    {
        ManageMenu();
    }

    private void ManageMenu()
    {
        if (_holder.activeSelf) CloseMenu();
        else OpenMenu();
    }

    private void OpenMenu()
    {
        int counter = 0;

        foreach (GameObject button in _stancePurchaseTemplates)
        {
            if (_stancePurchaseTemplates[counter].activeSelf)
            {
                OnSetSelectedButton(button);
                break;
            }
            else
            {
                counter++;
            }
        }

        _holder.SetActive(true);
    }

    private void CloseMenu()
    {
        _holder.SetActive(false);
    }

    public void UnlockVampireStance()
    {
        _stancePurchaseTemplates[0].SetActive(false);
        CloseMenu();
        OnUnlockStance(StanceType.Vampire);
    }

    public void UnlockOrcStance()
    {
        _stancePurchaseTemplates[1].SetActive(false);
        CloseMenu();
        OnUnlockStance(StanceType.Orc);
    }

    public void UnlockGhostStance()
    {
        _stancePurchaseTemplates[2].SetActive(false);
        CloseMenu();
        OnUnlockStance(StanceType.Ghost);
    }

    private void OnUnlockStance(StanceType stanceType)
    {
        UnlockStance?.Invoke(stanceType);
    }
}
