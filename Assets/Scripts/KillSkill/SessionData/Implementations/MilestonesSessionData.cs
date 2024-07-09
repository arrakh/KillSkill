using System.Collections.Generic;
using Unity.VisualScripting;

namespace KillSkill.SessionData.Implementations
{
    public class MilestonesSessionData : ISessionData
    {
        private HashSet<string> milestones = new ();

        public void Add(string milestoneKey) => milestones.Add(milestoneKey);

        public bool TryAdd(string milestoneKey)
        {
            if (milestones.Contains(milestoneKey)) return false;
            Add(milestoneKey);
            return true;
        }

        public bool Has(string milestoneKey) => milestones.Contains(milestoneKey);

        public bool HasAll(params string[] milestoneKeys)
        {
            foreach (var key in milestoneKeys)
                if (!Has(key)) return false;

            return true;
        }

        public bool HasAny(params string[] milestoneKeys)
        {
            foreach (var key in milestoneKeys)
                if (!Has(key)) return true;

            return false;
        }
    }
}