using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.HeavyKnight
{
    public class HeavyParrySkill : Skill, IGlobalCooldownSkill
    {
        private const float INCOMING_DAMAGE_REDUCE = 0.8f;
        private const float PARRY_DURATION = 0.4f;
        private const float STAGGER_DURATION = 5f;
        private const float OPEN_WIDE_MULTIPLIER = 2f;
        private const float OPEN_WIDE_DURATION = 5f;
        
        protected override float CooldownTime => 0.8f;

        private Character casterChar;
        private Character targetChar;

        public override bool CanExecute(Character caster)
            => !caster.StatusEffects.Has<ParryingStatusEffect>();

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-heavy-parry"),
            name = "Heavy Parry",
            description = "Grants <u>Parrying</u>.\n" +
                          $"On SUCCESS: Reduce incoming damage by x{INCOMING_DAMAGE_REDUCE:F1}, Target becomes <u>Staggered</u>\n" +
                          "On FAILED: User becomes <u>Open Wide</u>",
            extraDescription = $"-<u>Parrying</u>: {ParryingStatusEffect.StandardDescription()}\n" +
                               $"-<u>Staggered</u>: {StaggerStatusEffect.StandardDescription()}\n" +
                               $"-<u>Open Wide</u>: {OpenWideStatusEffect.StandardDescription(OPEN_WIDE_MULTIPLIER)}",
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 3, archetypeId = Archetypes.HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 50}
            }
        };

        public override void Execute(Character caster, Character target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new ParryingStatusEffect(caster, OnParryResult, PARRY_DURATION, INCOMING_DAMAGE_REDUCE));
        }

        private void OnParryResult(bool success)
        {
            if (success) targetChar.StatusEffects.Add(new StaggerStatusEffect(STAGGER_DURATION));
            else casterChar.StatusEffects.Add(new OpenWideStatusEffect(OPEN_WIDE_MULTIPLIER, OPEN_WIDE_DURATION));
        }
    }
}