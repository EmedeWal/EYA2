using UnityEngine;

public class LifetimeManager : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _lifeTime = 1f;

    private void Start()
    {
        Invoke(nameof(DestroyInstance), _lifeTime);
    }

    private void DestroyInstance()
    {
        Destroy(gameObject);
    }
}
