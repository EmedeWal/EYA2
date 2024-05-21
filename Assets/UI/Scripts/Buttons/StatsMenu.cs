using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _elements;

    // Do this in awake and ondestroy because the start of the pause menu disables this first elsewise.
    private void Awake()
    {
        StancePurchaseMenu.UnlockStance += StatsMenu_UnlockStance;
    }

    private void Start()
    {
        foreach (var element in _elements)
        {
            element.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        StancePurchaseMenu.UnlockStance -= StatsMenu_UnlockStance;
    }

    private void StatsMenu_UnlockStance(StanceType stanceType)
    {
        switch (stanceType)
        {
            case StanceType.Vampire:
                ActivateElement(0);
                break;

            case StanceType.Orc:
                ActivateElement(1);
                break;

            case StanceType.Ghost:
                ActivateElement(2);
                break;
        }
    }

    private void ActivateElement(int index)
    {
        _elements[index].SetActive(true);
    }
}
