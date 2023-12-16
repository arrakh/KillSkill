using Actors;

namespace Skills
{
    public class HealSkill : Skill
    {
        private float heal;
        
        public override SkillDescription Description => new()
        {
            name = "Heal",
            description = $"Heals for {heal} hp",
            icon = SkillIconDatabase.Get("heal").icon
        };

        public HealSkill(float cooldown, float heal) : base(cooldown)
        {
            this.heal = heal;   
        }

        public override void Execute(Character caster, Character target)
        {
            target.TryHeal(caster, heal);
        }
    }
}