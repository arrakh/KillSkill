using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class DualStrikeSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 0.8f;
        private ICharacter casterChar, targetChar;

        [Configurable] private Range damage = new (4f, 6f);
        [Configurable] private float castDuration = 0.5f;

        public override SkillMetadata Metadata => new()
        {
            name = "Dual Strike",
            description = $"Strike twice dealing {damage} damage each",
            icon = SpriteDatabase.Get("skill-dual-strike")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new RepeatCastingStatusEffect(castDuration / 2f, 2, OnCastCycle));
        }

        private void OnCastCycle(int obj)
        {
            casterChar.AnimateMoveTowards(targetChar, castDuration, Ease.OutQuart, 1/8f);
            casterChar.Animator.PlayFlipBook("attack");

            targetChar.TryDamage(casterChar, damage.GetRandomRounded());
        }
    }
}