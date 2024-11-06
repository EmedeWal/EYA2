using UnityEngine;

namespace EmeWillem
{
    [CreateAssetMenu(fileName = "Teleport Attack Data", menuName = "Scriptable Object/Data/Attack Data/Teleport")]
    public class TeleportAttackData : BaseAttackData
    {
        [Header("DISTANCE")]
        [SerializeField] private float _teleportDistance = 20f;
        private CreatureAI _creature;

        public void Init(CreatureAI creature)
        {
            _creature = creature;
        }

        public override void Attack(Transform target)
        {
            _creature.SetState(new TeleportState(_creature, target, _teleportDistance));
        }
    }
}