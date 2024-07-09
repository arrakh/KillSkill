using System.Collections.Generic;

namespace KillSkill.Characters
{
    public interface ICataloguedEnemy
    {
        //todo: PUT INTO SOME KIND OF SERIALIZED CATALOGUE DATA!

        public int CatalogOrder { get; }
        
        public IEnumerable<string> RequiredMilestones { get; }
    }
}