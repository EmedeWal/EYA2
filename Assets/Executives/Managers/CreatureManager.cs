//namespace EmeWillem
//{
//    using System.Collections.Generic;
//    using UnityEngine;
//    using System;

//    public class CreatureManager : MonoBehaviour
//    {
//        private float _delta;

//        public Transform PlayerTransform;

//        [Header("CREATURE PREFABS")]
//        [SerializeField] private List<CreatureAI> _creaturePrefabList = new();
//        private List<CreatureAI> _activeCreatureList = new();

//        private LayerMask _creatureLayer;
//        private LayerMask _targetLayer;

//        public static event Action<CreatureAI> CreatureDeath;

//        private AudioSystem _audioSystem;

//        public void Init()
//        {
//            _creatureLayer = LayerMask.GetMask("DamageCollider");
//            _targetLayer = LayerMask.GetMask("Controller");

//            _audioSystem = AudioSystem.Instance;

//            CollectCreatures();
//        }

//        public void FixedTick(float delta)
//        {
//            _delta = delta;

//            for (int i = 0; i < _activeCreatureList.Count; i++)
//            {
//                _activeCreatureList[i].FixedTick(_delta);
//            }
//        }

//        public void LateTick(float delta)
//        {
//            _delta = delta;

//            for (int i = 0; i < _activeCreatureList.Count; i++)
//            {
//                _activeCreatureList[i].LateTick(_delta);
//            }
//        }

//        public void Cleanup()
//        {
//            for (int i = _activeCreatureList.Count - 1; i >= 0; i--)
//            {
//                RemoveCreature(_activeCreatureList[i]);
//            }
//        }

//        private void AddCreature(CreatureAI creature)
//        {
//            _activeCreatureList.Add(creature);
//            creature.gameObject.tag = "Enemy";
//            creature.Init(_creatureLayer, _targetLayer, PlayerTransform);
//            creature.Health.HealthExhausted += CreatureManager_ValueExhausted;
//        }

//        private void RemoveCreature(CreatureAI creature)
//        {
//            creature.Health.HealthExhausted -= CreatureManager_ValueExhausted;
//            _activeCreatureList.Remove(creature);
//            creature.Cleanup();
//            Destroy(creature);
//        }

//        private void CreatureManager_DeathAnimationFinished(BaseAnimatorManager animatorManager)
//        {
//            animatorManager.DeathAnimationFinished -= CreatureManager_DeathAnimationFinished;
//            Destroy(animatorManager.gameObject);
//        }

//        private void CreatureManager_ValueExhausted(GameObject creatureObject)
//        {
//            CreatureAI creature = creatureObject.GetComponent<CreatureAI>();

//            if (creature.TryGetComponent(out AudioSource source))
//            {
//                _audioSystem.PlayAudio(source, source.clip, source.volume);
//            }

//            creature.AnimatorManager.DeathAnimationFinished += CreatureManager_DeathAnimationFinished;
//            creature.AnimatorManager.ForceCrossFade("Death");
//            creature.Locomotion.StopAgent(true);
//            OnCreatureDeath(creature);
//            RemoveCreature(creature);
//        }

//        private void OnCreatureDeath(CreatureAI creature)
//        {
//            CreatureDeath?.Invoke(creature);
//        }

//        private void CollectCreatures()
//        {
//            CreatureAI[] creatureAIArray = FindObjectsByType<CreatureAI>(FindObjectsSortMode.None);
//            foreach (CreatureAI creatureAI in creatureAIArray) AddCreature(creatureAI);
//        }
//    }
//}