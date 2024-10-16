using System.Collections;
using UnityEngine;

public class FreezingExplosion : AreaOfEffect
{
    [Header("VFX")]
    [SerializeField] private VFX _freezeVFX;

    [Header("VARIABLES")]
    [SerializeField] private float _slowPercentage;
    [SerializeField] private float _slowDuration;

    protected override void Effect(Collider hit)
    {
        if (hit.TryGetComponent(out AnimatorManager animatorManager))
        {
            Transform transform = hit.transform;
            VFXManager.Instance.AddMovingVFX(_freezeVFX, transform, _slowDuration);

            float originalMovementSpeed = animatorManager.MovementSpeed;
            float originalAttackSpeed = animatorManager.AttackSpeed;

            animatorManager.MovementSpeed *= _slowPercentage / 100f;
            animatorManager.AttackSpeed *= _slowPercentage / 100f;

            StartCoroutine(RestoreSpeedAfterDuration(animatorManager, originalMovementSpeed, originalAttackSpeed));
        }
    }

    private IEnumerator RestoreSpeedAfterDuration(AnimatorManager animatorManager, float originalMovementSpeed, float originalAttackSpeed)
    {
        yield return new WaitForSeconds(_slowDuration);

        animatorManager.MovementSpeed = originalMovementSpeed;
        animatorManager.AttackSpeed = originalAttackSpeed;
    }
}
