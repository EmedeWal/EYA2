using UnityEngine;

public class HealthHelper : MonoBehaviour
{
    private Health _health;

    public float MaxHealth = 100;
    public float CurrentHealth = 90;

    public bool ShouldDestroy = false;

    private void Awake()
    {
        _health = GetComponent<Health>();

        _health.Init(MaxHealth, CurrentHealth);

        _health.ValueExhausted += HealthHelper_ValueExhausted;
    }

    private void LateUpdate()
    {
        _health.LateTick(Time.deltaTime);
    }

    private void HealthHelper_ValueExhausted()
    {
        if (ShouldDestroy)
        {
            Destroy(gameObject);
        }
    }
}
