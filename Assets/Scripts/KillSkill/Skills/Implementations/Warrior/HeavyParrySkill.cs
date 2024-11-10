using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.HeavyKnight
{
    public class HeavyParrySkill : Skill, IGlobalCooldownSkill
    {
        private const float INCOMING_DAMAGE_REDUCE = 0.8f;
        private const float PARRY_DURATION = 0.5f;
        private const float DAZE_DURATION = 8f;
        private const float OPEN_WIDE_MULTIPLIER = 1.4f;
        private const float OPEN_WIDE_DURATION = 5f;

        [Configurable] private float parryDuration = 0.5f;
        [Configurable] private float dazeDuration = 0.5f;

        protected override float CooldownTime => 0.8f;

        private ICharacter casterChar;
        private ICharacter targetChar;

        public override bool CanExecute(ICharacter caster)
            => !caster.StatusEffects.Has<ParryingStatusEffect>();

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-heavy-parry"),
            name = "Heavy Parry",
            description = "Grants <u>Parrying</u>.\n\n" +
                          $"On SUCCESS: Reduce incoming damage by x{INCOMING_DAMAGE_REDUCE:F1}, Target becomes <u>Dazed</u>\n\n" +
                          "On FAILED: User becomes <u>Open Wide</u>",
            extraDescription = $"-<u>Parrying</u>: {ParryingStatusEffect.StandardDescription()}\n" +
                               $"-<u>Dazed</u>: {DazedStatusEffect.StandardDescription()}\n" +
                               $"-<u>Open Wide</u>: {OpenWideStatusEffect.StandardDescription(OPEN_WIDE_MULTIPLIER)}",
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 3, archetypeId = Archetypes.LEGACY_HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 50},
                {GameResources.MEDALS, 1},
            }
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new ParryingStatusEffect(caster, OnParryResult, PARRY_DURATION, INCOMING_DAMAGE_REDUCE));
        }

        private void OnParryResult(bool success)
        {
            if (success) targetChar.StatusEffects.Add(new DazedStatusEffect(DAZE_DURATION));
            else casterChar.StatusEffects.Add(new OpenWideStatusEffect(OPEN_WIDE_MULTIPLIER, OPEN_WIDE_DURATION));
        }
    }
}