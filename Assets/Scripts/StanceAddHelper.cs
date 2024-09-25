using UnityEngine.Events;
using UnityEngine;

public class StanceAddHelper : MonoBehaviour
{
    public UnityEvent IncreaseTier;
    public StanceType StanceType;

    public void AddStance()
    {
        PlayerStanceManager.Instance.UnlockStance(StanceType);
    }

    public void OnIncreaseTier()
    {
        IncreaseTier.Invoke();
    }
}
