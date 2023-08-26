using Actors;

namespace Skills
{
    public class HealSkill : Skill
    {
        private float heal;
        
        public override string DisplayName => "Heal";

        public HealSkill(float cooldown, float heal) : base(cooldown)
        {
            this.heal = heal;   
        }

        public override void Execute(Character caster, Character target)
        {
            target.Heal(caster, heal);
        }
    }
}