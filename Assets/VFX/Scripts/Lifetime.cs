using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _lifeTime = 1f;
    [SerializeField] private bool _countDownOnStart = false;

    private void Start()
    {
        if (_countDownOnStart)
        {
            DestroyAfterDelay(_lifeTime);
        }
    }

    public void DestroyAfterDelay(float delay)
    {
        Invoke(nameof(DestroyInstance), delay);
    }

    private void DestroyInstance()
    {
        Destroy(gameObject);
    }
}
