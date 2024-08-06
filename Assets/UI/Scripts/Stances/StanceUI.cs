using UnityEngine;

public class StanceUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject[] _stanceIcons;

    private void OnEnable()
    {
        Stance.UpdateStance += StanceUI_UpdateStance;
    }

    private void OnDisable()
    {
        Stance.UpdateStance -= StanceUI_UpdateStance;
    }

    private void Start()
    {
        DisableUI();
    }

    protected virtual void StanceUI_UpdateStance(StanceType stanceType)
    {
        UpdateUI(stanceType);
    }

    protected void UpdateUI(StanceType stanceType)
    {
        switch (stanceType)
        {
            case StanceType.Vampire:
                ActivateUI(0);
                break;

            case StanceType.Orc:
                ActivateUI(1);
                break;

            case StanceType.Ghost:
                ActivateUI(2);
                break;
        }
    }

    private void ActivateUI(int position)
    {
        DisableUI();
        _stanceIcons[position].SetActive(true);
    }

    private void DisableUI()
    {
        foreach (var icon in _stanceIcons) icon.SetActive(false);
    }
}
