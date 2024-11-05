using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 250f;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();

        _health.Init(_maxHealth, _maxHealth);
        GetComponent<LockTarget>().Init();
    }

    private void LateUpdate()
    {
        _health.LateTick(Time.deltaTime);
    }
}