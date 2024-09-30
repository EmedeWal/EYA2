using UnityEngine.Events;
using UnityEngine;

public class StanceAddHelper : MonoBehaviour
{
    public UnityEvent IncreaseTier;
    public StanceType StanceType;

    public PlayerStats PlayerStats;
    public Stat StatToModify;
    public float Increment;

    public Health PlayerHealth;

    public void OnIncreaseTier()
    {
        IncreaseTier.Invoke();
    }

    public void ChangeStat()
    {
        PlayerStats.IncrementStat(StatToModify, Increment);
    }

    public void DamagePlayer()
    {
        PlayerHealth.TakeDamage(gameObject, 10);
    }
}
