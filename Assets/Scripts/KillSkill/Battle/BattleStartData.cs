using KillSkill.Characters;

namespace KillSkill.Battle
{
    public readonly struct BattleStartData
    {
        public readonly INpcDefinition npcDefinition;
        public readonly string levelId;

        public BattleStartData(INpcDefinition npcDefinition, string levelId)
        {
            this.npcDefinition = npcDefinition;
            this.levelId = levelId;
        }
    }
}