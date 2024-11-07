using System.Collections.Generic;
using UnityEngine;
using System;

namespace EmeWillem
{
    public abstract class BaseAttackHandler : MonoBehaviour
    {
        public event Action<BaseAttackData> EnteredAttackingState;
        public event Action<BaseAttackData> LeftAttackingState;

        protected BaseAnimatorManager _AnimatorManager;

        private List<OffenseCollider> _offenseColliderList;

        public virtual void Init(List<OffenseCollider> offenseColliders)
        {
            _AnimatorManager = GetComponent<BaseAnimatorManager>();

            _offenseColliderList = offenseColliders;
        }

        public void EnterAttackingState(BaseAttackData attackData)
        {
            OnEnteredAttackingState(attackData);

            // Play audio or sumthing
        }

        public void LeaveAttackingState(BaseAttackData attackData)
        {
            OnLeftAttackingState(attackData);
        }

        public void AttackStart(string data)
        {
            data = data.Replace(" ", "");
            string[] parameters = data.Split(',');
            int damage = int.Parse(parameters[0]);
            int stagger = int.Parse(parameters[1]);
            string offenseColliderName = parameters[2];

            foreach (OffenseCollider offenseCollider in _offenseColliderList)
            {
                if (offenseCollider.Name == offenseColliderName)
                {
                    offenseCollider.Activate(damage, stagger);
                    break;
                }
            }

            _AnimatorManager.SetBool("CanRotate", false);
        }

        public void AttackEnd(string offenseColliderName)
        {
            offenseColliderName = offenseColliderName.Replace(" ", "");
            foreach (OffenseCollider offenseCollider in _offenseColliderList)
            {
                if (offenseCollider.Name == offenseColliderName)
                {
                    offenseCollider.Deactivate();
                    break;
                }
            }
        }

        private void OnEnteredAttackingState(BaseAttackData attackData)
        {
            EnteredAttackingState?.Invoke(attackData);
        }

        private void OnLeftAttackingState(BaseAttackData attackData)
        {
            LeftAttackingState?.Invoke(attackData);
        }
    }
}