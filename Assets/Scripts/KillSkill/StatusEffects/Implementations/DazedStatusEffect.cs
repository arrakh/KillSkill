using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class DazedStatusEffect : TimedStatusEffect, IModifyIncomingDamage, IPreventSkillExecution
    {
        public DazedStatusEffect(float duration) : base(duration)
        {
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-dazed"),
            name = "Dazed",
            description = StandardDescription()
        };

        public override void OnAdded(Character target)
        {
            base.OnAdded(target);
            target.StatusEffects.TryRemove<CastingStatusEffect>();
            target.StatusEffects.TryRemove<StancingStatusEffect>();
        }

        public void ModifyDamage(Character damager, Character target, ref double damage)
        {
            target.StatusEffects.Remove<DazedStatusEffect>();
        }

        public static string StandardDescription() => "Prevents triggering any ability until timer runs out or damaged";
    }
}