using Database;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using Skills;
using StatusEffects;

namespace KillSkill.Skills.Implementations.Fighter
{
    public class SlashSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 2f;
        
        private const float DAMAGE = 20f;
        private const float CAST_TIME = 0.3f;
        private Character casterChar, targetChar;

        public override SkillMetadata Metadata => new()
        {
            name = "Slash",
            description = $"Basic attack dealing {DAMAGE} damage",
            icon = SpriteDatabase.Get("skill-slash")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.FIGHTER);

        public override void Execute(Character caster, Character target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(CAST_TIME, OnDone, OnInterrupted));

            casterChar.AnimateMoveTowards(target, CAST_TIME, Ease.OutQuart, 1/8f);
            casterChar.Animator.PlayFlipBook("attack");
        }

        private void OnInterrupted()
        {
            casterChar.Animator.BackToPosition();
        }

        private void OnDone()
        {
            targetChar.TryDamage(casterChar, DAMAGE);
            casterChar.AnimateMoveTowards(targetChar, 0.15f, Ease.OutQuart, 1/5f, casterChar.Animator.BackToPosition);
            casterChar.Animator.PlayFlipBook("idle");
        }
    }
}