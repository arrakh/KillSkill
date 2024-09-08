namespace KillSkill.Characters
{
    public interface ICharacterFactory
    {
        public NpcCharacter CreateNpc(INpcDefinition npcType, bool isEnemy = true);
        public NpcCharacter CreateNpc<T>(bool isEnemy = true) where T : INpcDefinition;
    }
}