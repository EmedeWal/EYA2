namespace EmeWillem
{
    using UnityEngine;

    public class Dummy : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 1000;

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();

            _health.Init(_maxHealth);
            //GetComponent<LockTarget>().Init();
        }

        private void LateUpdate()
        {
            _health.LateTick();
        }
    }
}