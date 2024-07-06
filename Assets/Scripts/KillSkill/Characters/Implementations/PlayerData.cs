using KillSkill.Skills;

namespace KillSkill.Characters.Implementations
{
    public struct PlayerData : ICharacterData
    {
        public PlayerData(Skill[] skills)
        {
            Skills = skills;
        }

        public string Id => "player";
        public float Health => 400;
        public Skill[] Skills { get; private set;  }
    }
}