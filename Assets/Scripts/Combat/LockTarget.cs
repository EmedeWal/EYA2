using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        public class LockTarget : MonoBehaviour
        {
            public Transform Center { get; private set; }
            public Health Health { get; private set; }
            public float Offset { get; private set; }


            public void Init(Health health, float offset)
            {
                Center = transform;
                Health = health;
                Offset = offset;
            }
        }
    }
}