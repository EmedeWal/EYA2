using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    public void PlayVFXInChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log("Child found");

            if (transform.GetChild(i).TryGetComponent(out ParticleSystem particleSystem))
            {
                Debug.Log("PArticle system");
                particleSystem.Play();
            }
        }
    }
}
