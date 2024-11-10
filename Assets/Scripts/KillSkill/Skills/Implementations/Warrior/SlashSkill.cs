using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;
using StatusEffects;

namespace KillSkill.Skills.Implementations.Warrior
{
    public class SlashSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 2f;
        private ICharacter casterChar, targetChar;

        [Configurable] private Range damage = new (15f, 25f);
        [Configurable] private float castDuration = 0.3f;

        public override SkillMetadata Metadata => new()
        {
            name = "Slash",
            description = $"Basic attack dealing {damage} damage",
            icon = SpriteDatabase.Get("skill-slash")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.WARRIOR);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(castDuration, OnDone));

            casterChar.AnimateMoveTowards(target, castDuration, Ease.OutQuart, 1/8f);
            casterChar.Animator.PlayFlipBook("attack");
        }

        private void OnDone()
        {
            targetChar.TryDamage(casterChar, damage.GetRandomRounded());
            casterChar.AnimateMoveTowards(targetChar, 0.15f, Ease.OutQuart, 1/5f, casterChar.Animator.BackToPosition);
            casterChar.Animator.PlayFlipBook("idle");
        }
    }
}