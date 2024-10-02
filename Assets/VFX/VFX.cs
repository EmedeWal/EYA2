using UnityEngine;

public class VFX : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Transform _followTarget;
    private Transform _transform;

    public virtual void Activate(Transform followTarget)
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _followTarget = followTarget;
        _transform = transform;
    }

    public virtual void Tick()
    {
        if (_transform.position != _followTarget.position)
        {
            _transform.position = _followTarget.position;
        }
    }

    public virtual void Deactivate()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
        }
    }
}
