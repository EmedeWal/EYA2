using System.Collections;
using UnityEngine;
using System;

public class Bleed : MonoBehaviour
{
    // Variables
    private Health _health;

    // Events
    public event Action<int> BleedUpdate;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void InflictBleed(float damage, float duration)
    {
        StartCoroutine(BleedCoroutine(damage, duration));
    }

    private IEnumerator BleedCoroutine(float damage, float duration)
    {
        OnBleedUpdate(1);

        int ticks = Mathf.CeilToInt(duration / 0.1f);
        float damagePerTick = damage / ticks;

        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(0.1f);
            _health.TakeDamage(gameObject, damagePerTick);
        }

        OnBleedUpdate(-1);
    }

    private void OnBleedUpdate(int modification)
    {
        BleedUpdate?.Invoke(modification);
    }
}
