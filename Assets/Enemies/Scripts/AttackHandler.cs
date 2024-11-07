using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    namespace AI
    {
        public class AttackHandler : BaseAttackHandler
        {
            [HideInInspector] public AttackData AttackData;

            [Header("ATTACKS")]
            public List<AttackData> AttackDataList = new();

            public void SelectRandomAttack(List<AttackData> attackDataList = null)
            {
                if (attackDataList == null || attackDataList.Count == 0)
                {
                    attackDataList = AttackDataList;
                }

                int randomIndex = Random.Range(0, attackDataList.Count);
                AttackData = attackDataList[randomIndex];
            }
        }
    }
}