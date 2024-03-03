using KillSkill.Characters;
using Skills;

namespace KillSkill.Skills
{
    //todo: MAKE THIS AN ISKILL
    //HAVE IT BE HANDLED IN SKILLHANDLER IN CHARACTER
    //SKILLHANDLER WILL HAVE WRAPPER THAT TAKES AWAY THE IMPLEMENTATION OF A SKILL
    //HAVE SKILL HAVE INTERFACES LIKE ICOOLDOWNSKILL
    public abstract class Skill
    {
        private readonly Timer cd;
        
        public virtual string DisplayName => string.Empty;
        public virtual SkillMetadata Metadata => SkillMetadata.Empty;
        
        public Timer Cooldown => cd;

        protected abstract float CooldownTime { get; }
        public virtual CatalogEntry CatalogEntry => CatalogEntry.NonPurchasable;

        protected Skill()
        {
            cd = new Timer(CooldownTime, false);
        }

        public void UpdateCooldown(float deltaTime)
        {
            cd.Update(deltaTime);
        }

        public bool CanExecute(Character caster) => !cd.IsActive && caster.CanCastAbility(this);

        public void TriggerCooldown() => cd.Reset();

        public virtual void Execute(Character caster, Character target) { }
    }
}
