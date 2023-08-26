using Actors;

namespace Skills
{
    public class StunSkill : Skill
    {
        public float duration;
        
        public StunSkill(float duration, float cooldown) : base(cooldown)
        {
            this.duration = duration;
        }

        public override void Execute(Character caster, Character target)
        {
            
        }
    }
}