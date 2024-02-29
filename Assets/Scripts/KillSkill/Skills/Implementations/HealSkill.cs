using System.Collections.Generic;
using Actors;
using Database;
using KillSkill.Constants;

namespace Skills
{
    public class HealSkill : Skill
    {
        private const float HEAL_AMOUNT = 35f;
        protected override float CooldownTime => 12f;
        
        public override CatalogEntry CatalogEntry => new(Archetypes.HEALER, new Dictionary<string, double>()
        {
            { GameResources.COINS, 30 }
        });

        public override SkillMetadata Metadata => new()
        {
            name = "Heal",
            description = $"Heals for {HEAL_AMOUNT} hp",
            icon = SpriteDatabase.Get("skill-heal")
        };


        public override void Execute(Character caster, Character target)
        {
            caster.TryHeal(caster, HEAL_AMOUNT);
        }
    }
}