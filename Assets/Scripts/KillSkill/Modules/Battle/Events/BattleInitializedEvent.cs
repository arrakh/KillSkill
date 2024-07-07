using KillSkill.Battle;
using KillSkill.Characters;

namespace KillSkill.Modules.Battle.Events
{
    public struct BattleInitializedEvent
    {
        public ICharacter player, enemy;
        public BattleLevel level;

        public BattleInitializedEvent(ICharacter player, ICharacter enemy, BattleLevel level)
        {
            this.player = player;
            this.enemy = enemy;
            this.level = level;
        }
    }
}