using UnityEngine;
using System.Linq;
using System;

namespace EmeWillem
{
    namespace Utilities
    {
        public static class AttackEventHelper
        {
            public static Func<GameObject, Vector3, bool> OnBlockCheck;

            public static bool CheckBlocking(GameObject targetObject, Vector3 attackerPosition)
            {
                if (OnBlockCheck != null)
                {
                    foreach (Func<GameObject, Vector3, bool> blockCheck in OnBlockCheck.GetInvocationList().Cast<Func<GameObject, Vector3, bool>>())
                    {
                        if (blockCheck.Invoke(targetObject, attackerPosition))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
}
