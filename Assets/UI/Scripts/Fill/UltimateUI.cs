using UnityEngine;
using System.Collections;

public class UltimateUI : FillUI
{
    //private void Start()
    //{
    //    ManageTimerImage(false);
    //}

    private void OnEnable()
    {
        //StancePurchaseMenu.UnlockStance += UltimateUI_UnlockStance;
        //StanceBase.UltimateStart += UltimateUI_UltimateStart;
    }

    private void OnDisable()
    {
        //StancePurchaseMenu.UnlockStance -= UltimateUI_UnlockStance;
        //StanceBase.UltimateStart -= UltimateUI_UltimateStart;
    }

    //private void UltimateUI_UnlockStance(StanceType stanceType)
    //{
    //    ManageTimerImage(true);
    //}

    private void UltimateUI_UltimateStart(float duration)
    {
        StartCoroutine(UltimateDurationCoroutine(duration));
    }

    private IEnumerator UltimateDurationCoroutine(float duration)
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

    //private void ManageTimerImage(bool active)
    //{
    //    TimerImage.gameObject.SetActive(active);
    //}
}
