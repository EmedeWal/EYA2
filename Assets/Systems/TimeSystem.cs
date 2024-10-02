using UnityEngine;

public class TimeSystem : SingletonBase
{
    #region Singleton
    public static TimeSystem Instance;

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CurrentTimeScale = Time.timeScale;
        _previousTimeScale = CurrentTimeScale;
    }
    #endregion

    public float CurrentTimeScale { get; private set; }
    private float _previousTimeScale;

    public void SetTimeScale(float scale)
    {
        SetLocalTimeScale(scale);
    }

    public void ResetTimeScale()
    {
        SetLocalTimeScale(1);
    }

    public void RevertToPreviousTimeScale()
    {
        SetLocalTimeScale(_previousTimeScale);
    }

    private void SetLocalTimeScale(float scale)
    {
        _previousTimeScale = CurrentTimeScale;
        CurrentTimeScale = scale;
        Time.timeScale = scale;
    }
}
