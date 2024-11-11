using DG.Tweening;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class AssassinateSkill : Skill
    {
        protected override float CooldownTime => 30f;

        [Configurable] private float stanceDuration = 1.2f;
        [Configurable] private float stanceStaggerLimit = 25f;
        [Configurable] private float damageFactor = 8f;

        private float finalDamage;
        private ICharacter casterChar;
        private ICharacter targetChar;

        public override SkillMetadata Metadata => new()
        {
            name = "Assassinate",
            description = $"Enter </u>Stancing</u> for {stanceDuration} seconds, then attack as many as target's total Bleed and Poison for {damageFactor} damage each",
            extraDescription = $"- <u>Stancing</u>: {StancingStatusEffect.StandardDescription(stanceStaggerLimit)}",
            icon = SpriteDatabase.Get("skill-assassinate")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            finalDamage = GetDamage(target);
            caster.StatusEffects.Add(new StancingStatusEffect(OnDoneStancing, stanceStaggerLimit, stanceDuration));
        }

        private float GetDamage(ICharacter target)
        {
            int count = 0;
            if (target.Resources.TryGet(out Bleed bleed)) count += bleed.Count;
            if (target.Resources.TryGet(out Poison poison)) count += poison.Count;
            return count * damageFactor;
        }

        private void OnDoneStancing()
        {
            targetChar.TryDamage(casterChar, finalDamage);
            casterChar.AnimateMoveTowards(targetChar, 0.2f, Ease.OutQuart, 1/6f, casterChar.Animator.BackToPosition);
            casterChar.Animator.PlayFlipBook("attack");
        }
    }
}