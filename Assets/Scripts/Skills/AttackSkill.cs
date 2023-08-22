using Actors;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public class AttackSkill : Skill
    {
        private float damage;

        public override string DisplayName => "Attack";

        public AttackSkill(float cooldown, float damage) : base(cooldown)
        {
            this.damage = damage;
        }

        public override void Execute(Character caster, Character target)
        {
            target.Damage(damage);
        }
    }
}