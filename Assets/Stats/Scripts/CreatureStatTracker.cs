namespace EmeWillem
{
    using System.Collections.Generic;

    public class CreatureStatTracker
    {
        private Dictionary<Stat, float> _statChanges;
        private CreatureStatManager _statManager;

        public CreatureStatTracker(Dictionary<Stat, float> statChanges, CreatureStatManager statManager)
        {
            _statChanges = statChanges;
            _statManager = statManager;
        }

        public void IncrementStat(Stat stat, float value)
        {
            _statChanges[stat] += value;
            _statManager.IncrementStat(stat, value);
        }

        public void ResetStatChanges()
        {
            var statChangeCopy = new Dictionary<Stat, float>(_statChanges);

            foreach (var statChange in statChangeCopy)
            {
                if (statChange.Value != 0)
                {
                    _statManager.IncrementStat(statChange.Key, -statChange.Value);
                    _statChanges[statChange.Key] = 0;
                }
            }
        }

        public float GetStatChange(Stat stat)
        {
            return _statChanges[stat];
        }
    }
}