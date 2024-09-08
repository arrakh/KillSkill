using KillSkill.Characters;

namespace KillSkill.Modules.Battle.Events
{
    public class QueryCharacterRegistry
    {
        public readonly ICharacterRegistry registry;

        public QueryCharacterRegistry(ICharacterRegistry registry)
        {
            this.registry = registry;
        }
    }
}