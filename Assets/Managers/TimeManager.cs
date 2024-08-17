using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    public static TimeManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private float _previousTimeScale = 1.0f;

    public void SetTimeScale(float scale)
    {
        _previousTimeScale = Time.timeScale;
        Time.timeScale = scale;
    }

    public void ResetTimeScaleToDefault()
    {
        Time.timeScale = 1f;
    }

    public void RevertToPreviousTimeScale()
    {
        Time.timeScale = _previousTimeScale;
    }
}
