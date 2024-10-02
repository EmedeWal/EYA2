using UnityEngine;

public class ParticleEmissionModifier : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emissionModule;

    public void Init(float emissionRate)
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _emissionModule = _particleSystem.emission;
        _emissionModule.rateOverTime = emissionRate;
    }

    public void Tick(float emissionRate)
    {
        _emissionModule.rateOverTime = emissionRate;
    }
}
