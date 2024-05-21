using UnityEngine;
using System.Collections;

public class UltimateUI : FillUI
{
    private void Start()
    {
        ManageTimerImage(false);
    }

    private void OnEnable()
    {
        StancePurchaseMenu.UnlockStance += UltimateUI_UnlockStance;
    }

    private void OnDisable()
    {
        StancePurchaseMenu.UnlockStance -= UltimateUI_UnlockStance;
    }

    private void UltimateUI_UnlockStance(StanceType stanceType)
    {
        ManageTimerImage(true);
    }

    public void Duration(float duration)
    {
        StartCoroutine(DurationCoroutine(duration));
    }

    private IEnumerator DurationCoroutine(float duration)
    {
        float remainingTime = duration;

        SetMaxValue(remainingTime);

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            SetCurrentValue(remainingTime);
            yield return null;
        }
    }

    private void ManageTimerImage(bool active)
    {
        TimerImage.gameObject.SetActive(active);
    }
}
