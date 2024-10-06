using UnityEngine;

public class StanceSmoke : VFX
{
    private ParticleSystem _particleSystem;

    public override void Init(Transform followTarget)
    {
        base.Init(followTarget);

        _particleSystem = GetComponent<ParticleSystem>();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        _particleSystem.Stop();
    }
}
