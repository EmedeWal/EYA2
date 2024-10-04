using System.Collections.Generic;
using UnityEngine;

public class VFXEmission : MonoBehaviour
{
    private List<ParticleSystem> _particleSystems = new();
    private List<ParticleSystem.EmissionModule> _emissionModules = new();

    public void Init(float emissionRate)
    {
        _particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
        
        if (TryGetComponent(out ParticleSystem ps))
        {
            _particleSystems.Add(ps);
        }

        foreach (ParticleSystem particleSystem in _particleSystems)
        {
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            _emissionModules.Add(emissionModule);
            emissionModule.rateOverTime = emissionRate;
        }
    }

    public void Tick(float emissionRate)
    {
        for (int i = 0; i < _emissionModules.Count; i++)
        {
            var emissionModule = _particleSystems[i].emission;
            emissionModule.rateOverTime = emissionRate;
        }
    }
}
