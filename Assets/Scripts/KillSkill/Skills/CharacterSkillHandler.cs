using System;
using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Utility;
using Skills;
using StatusEffects;
using Unity.Netcode;

namespace KillSkill.Skills
{
    public partial class CharacterSkillHandler : NetworkBehaviour, IDisposable, ICharacterSkillHandler
    {
        public void Initialize(Type[] ownerSkills, ICharacter owner)
        {
            skills = GenerateSkills(ownerSkills);
            statusEffects = owner.StatusEffects;
            character = owner;
            
            for (var index = 0; index < ownerSkills.Length; index++)
            {
                var skill = ownerSkills[index];
                if (skill.IsEmpty()) continue;

                var type = skill.GetType();
                skillIndexes[type] = index;

                DetectAndRegisterOnExecutedCallbacks(type);
            }
        }

        private Skill[] GenerateSkills(Type[] ownerSkills)
        {
            var activatedSkills = new Skill[ownerSkills.Length];
            
            for (var i = 0; i < ownerSkills.Length; i++)
            {
                var skill = ownerSkills[i];
                if (skill.IsEmpty()) activatedSkills[i] = null;
                else
                {
                    var instance = Activator.CreateInstance(skill) as Skill;
                    activatedSkills[i] = instance;
                }
            }

            return activatedSkills;
        }

        public event Action<Skill> OnAnySkillExecuted; 

        public float CooldownMultiplier { get; private set; }

        public Timer GlobalCooldown => globalCd;

        private Skill[] skills;
        private Dictionary<Type, int> skillIndexes = new();

        private Timer globalCd = new(0, false);

        private IStatusEffectsHandler statusEffects;
        private ICharacter character;

        public void InitializeSkills()
        {
            foreach (var skill in skills)
                if (skill != null) skill.OnInitialize(character);
        }

        public void UpdateHandler(float deltaTime)
        {
            globalCd.Update(deltaTime);

            foreach (var skill in skills)
                skill?.UpdateCooldown(deltaTime);
        }

        public bool CanCast(Skill skill)
        {
            if (!character.IsAlive) return false;
            
            bool canCast = !statusEffects.Has<IPreventSkillExecution>();

            if (skill is IGlobalCooldownSkill) return canCast && !globalCd.IsActive;

            return canCast;
        }

        public bool CanCast(int index)
        {
            if (index < 0 || index >= skills.Length) return false;
            var skill = skills[index];
            if (skill.IsEmpty()) return false;
            return CanCast(skill);
        }

        public bool CanCast<T>()
        {
            if (!skillIndexes.TryGetValue(typeof(T), out var index)) return false;
            var skill = skills[index];
            if (skill.IsEmpty()) return false;
            return CanCast(skill);
        }

        public void SetCooldownSpeed(float multiplier)
        {
            CooldownMultiplier = multiplier;
            globalCd.SetSpeed(multiplier);
            foreach (var skill in skills)
                skill?.Cooldown.SetSpeed(multiplier);
        }
        
        public Skill Get(int index) => skills[index];
        public Skill[] GetAll() => skills;

        public bool TryGet(int index, out Skill skill)
        {
            skill = default;
            if (index >= skills.Length) return false;
            skill = skills[index];
            return true;
        }

        public bool TryGetIndex<T>(out int skillIndex)
            => skillIndexes.TryGetValue(typeof(T), out skillIndex);

        public void Execute<T>(ICharacter target) where T : Skill
        {
            if (!skillIndexes.TryGetValue(typeof(T), out var index))
                throw new Exception($"Trying to execute {typeof(T)} but character does not have it!");
            
            Execute(index, target);
        }

        public void Execute(int index, ICharacter target)
        {
            if (index >= skills.Length)
                throw new Exception($"Trying to execute Skill index {index} but there are only {skills.Length} skills!");
            
            var skill = skills[index];
            if (!skill.CanExecute(character)) return;
            
            skill.Execute(character, target);
            skill.TriggerCooldown();
            
            if (skill is IGlobalCooldownSkill) globalCd.Set(skill.Cooldown.Duration);
            
            OnAnySkillExecuted?.Invoke(skill);
            InvokeCallbacks(character, target, skill);
            foreach (var s in skills)
                if (s != null && s is IAnySkillExecutedCallback cb) 
                    cb.OnAnyExecuted(character, target, skill);
        }

        public void Dispose()
        {
            
        }
    }
}