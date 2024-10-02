using UnityEngine;

public class FreezingExplosion : AreaOfEffect
{
    [Header("VARIABLES")]
    [SerializeField] private float _slowPercentage;
    [SerializeField] private float _slowDuration;

    protected override void Effect(Collider hit)
    {
        Debug.Log("Slowing hit. Not implemented");
    }
}
