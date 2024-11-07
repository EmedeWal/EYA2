
//namespace EmeWillem
//{
//    using System.Collections;
//    using UnityEngine;

//    public class BloodEruption : AreaOfEffect
//    {
//        private BleedingStats _bleedingStats;
//        private int _stacks;

//        public void InitBloodEruption(float radius, float delay, int stacks, LayerMask targetLayer, BleedingStats bleedingStats)
//        {
//            _bleedingStats = bleedingStats;
//            _stacks = stacks;

//            StartCoroutine(InitCoroutine());
//            IEnumerator InitCoroutine()
//            {
//                yield return new WaitForSeconds(delay);
//                base.Init(radius, targetLayer);
//            }
//        }

//        protected override void Effect(Collider hit)
//        {
//            if (hit.TryGetComponent(out BleedHandler bleedHandler))
//            {
//                bleedHandler.ApplyBleed(_bleedingStats, _stacks);
//            }
//        }
//    }
//}