using System.Collections.Generic;
using UnityEngine;

namespace EmeWillem
{
    public class CreatureStatManager : MonoBehaviour
    {
        private Dictionary<Stat, float> _stats;

        private CreatureAnimatorManager _animatorManager;
        private CreatureAttackHandler _attackHandler;
        private Health _health;

        public void Init()
        {
            _stats = new()
        {
            { Stat.DamageReduction, 0 },
            { Stat.AttackSpeedModifier, 1 },
            { Stat.AttackDamageModifier, 1 },
            { Stat.MovementSpeedModifier, 1 }
        };

            _animatorManager = GetComponent<CreatureAnimatorManager>();
            _attackHandler = GetComponent<CreatureAttackHandler>();
            _health = GetComponent<Health>();

            //_health.DamageReduction = _stats[Stat.DamageReduction];
            _animatorManager.AttackSpeed = _stats[Stat.AttackSpeedModifier];
            _attackHandler.DamageModifier = _stats[Stat.AttackDamageModifier];
            _animatorManager.MovementSpeed = _stats[Stat.MovementSpeedModifier];
        }

        public void Cleanup()
        {
            _stats.Clear();
        }

        public void IncrementStat(Stat stat, float value)
        {
            _stats[stat] += value;
            UpdateStat(stat, _stats[stat]);
        }

        public float GetStat(Stat stat)
        {
            return _stats[stat];
        }

        private void UpdateStat(Stat stat, float value)
        {
            switch (stat)
            {
                case Stat.DamageReduction:
                    //_health.DamageReduction = value;
                    break;

                case Stat.AttackSpeedModifier:
                    _animatorManager.AttackSpeed = value;
                    break;

                case Stat.AttackDamageModifier:
                    _attackHandler.DamageModifier = value;
                    break;

                case Stat.MovementSpeedModifier:
                    _animatorManager.MovementSpeed = value;
                    break;
            }
        }
    }

}