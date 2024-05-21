using UnityEngine;
using System.Collections;
using TMPro;

public abstract class PotionUI : FillUI
{
    [SerializeField] private TextMeshProUGUI text;
    private float _remainingTime;

    protected void UpdateCharges(int charges)
    {
        text.text = charges.ToString();
    }

    protected void StartRefillTime(float refillTime)
    {
        StopAllCoroutines();
        SetMaxValue(refillTime);
        StartCoroutine(CooldownCoroutine(refillTime));
    }

    protected void UpdateRefillTime(float newTime)
    {
        _remainingTime -= newTime;
    }

    private IEnumerator CooldownCoroutine(float time)
    {
        _remainingTime = time;

        while (true)
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime <= 0)
            {
                _remainingTime = 0;
                SetCurrentValue(_remainingTime);
                break;
            }

            SetCurrentValue(_remainingTime);
            yield return null;
        }
    }
}
