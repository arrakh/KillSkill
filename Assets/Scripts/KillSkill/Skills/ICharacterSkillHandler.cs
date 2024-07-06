using System;
using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface ICharacterSkillHandler
    {
        public event Action<Skill> OnAnySkillExecuted;
        
        public Timer GlobalCooldown { get; }

        public float CooldownMultiplier { get; }

        public bool CanCast(Skill skill);

        public bool CanCast(int index);

        public bool CanCast<T>();

        public void SetCooldownSpeed(float multiplier);

        public Skill Get(int index);

        public Skill[] GetAll();

        public bool TryGet(int index, out Skill skill);

        public bool TryGetIndex<T>(out int skillIndex);

        public void Execute<T>(ICharacter target) where T : Skill;

        public void Execute(int index, ICharacter target);
    }
}