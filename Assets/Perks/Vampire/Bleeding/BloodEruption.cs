using System.Collections;
using UnityEngine;

public class BloodEruption : AreaOfEffect
{
    private BleedingStats _bleedingStats;
    private int _stacks;

    public void InitBloodEruption(int stacks, BleedingStats bleedingStats, float radius, float delay, LayerMask targetLayer)
    {
        _stacks = stacks;
        _bleedingStats = bleedingStats;

        StartCoroutine(InitCoroutine());
        IEnumerator InitCoroutine()
        {
            yield return new WaitForSeconds(delay);
            base.Init(radius, targetLayer);
        }
    }

    protected override void Effect(Collider hit)
    {
        if (hit.TryGetComponent(out BleedHandler bleedHandler))
        {
            bleedHandler.ApplyBleed(_bleedingStats);
        }
    }
}