using UnityEngine;

public class StanceAddHelper : MonoBehaviour
{
    public StanceType StanceType;

    public void AddStance()
    {
        PlayerStanceManager.Instance.UnlockStance(StanceType);
    }
}
