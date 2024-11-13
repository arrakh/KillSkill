using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class DualStrikesSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 0.8f;
        private ICharacter casterChar, targetChar;

        [Configurable] private Range damage = new (4f, 6f);
        [Configurable] private float castDuration = 0.5f;

        public override SkillMetadata Metadata => new()
        {
            name = "Dual Strikes",
            description = $"Strikes twice dealing {damage} damage each",
            icon = SpriteDatabase.Get("skill-dual-strikes")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN, 0);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new RepeatCastingStatusEffect(castDuration / 2f, 2, OnCastCycle));
            MoveForward();
        }

        private void OnCastCycle(int cycle)
        {
            casterChar.Animator.PlayFlipBook("attack");

            targetChar.TryDamage(casterChar, damage.GetRandomRounded());
            
            if (cycle == 2) casterChar.Animator.BackToPosition();
            else MoveForward();
        }
        
        private void MoveForward() 
            => casterChar.AnimateMoveTowards(targetChar, castDuration, Ease.OutQuart, 1/6f);

    }
}