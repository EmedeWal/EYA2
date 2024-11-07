using EmeWillem.Utilities;
using UnityEngine;

namespace EmeWillem
{
    [RequireComponent (typeof(Collider))]
    public class DefenseCollider : MonoBehaviour
    {
        public GameObject Parent { get; private set; }

        private Health _health;
        private Posture _posture;

        public void Init(GameObject parent, Health health, Posture posture, LayerMask layer)
        {
            Parent = parent;

            _health = health;
            _posture = posture;

            gameObject.layer = Mathf.RoundToInt(Mathf.Log(layer.value, 2));
        }

        public int ProcessAttack(Vector3 attackerPosition, int damageValue, int staggerValue)
        {
            if (AttackEventHelper.CheckBlocking(Parent, attackerPosition))
            {
                return 0;
            }

            _posture.InflictStagger(staggerValue);
            return _health.RemoveHealth(damageValue);
        }
    }
}