using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    public void PlayVFXInChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ParticleSystem particleSystem))
            {
                particleSystem.Play();
            }
        }
    }

    public void StopVFXInChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ParticleSystem particleSystem))
            {
                particleSystem.Stop();
            }
        }
    }
}
