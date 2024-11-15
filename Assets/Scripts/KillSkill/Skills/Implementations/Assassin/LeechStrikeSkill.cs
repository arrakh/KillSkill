using DG.Tweening;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class LeechStrikeSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 4f;

        private ICharacter casterChar;
        private ICharacter targetChar;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Leech Strike",
            description = $"",
            icon = SpriteDatabase.Get("skill-dual-strikes")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN, 99);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            
            casterChar.AnimateMoveTowards(target, 1f, Ease.OutQuart, 1/8f);
            caster.StatusEffects.Add(new CastingStatusEffect(1f, OnDoneCasting));
        }

        public override bool CanExecute(ICharacter caster)
        {
            bool hasTarget = caster.Target != null;
            if (!hasTarget) return false;

            bool targetHasBleed = caster.Target.Resources.IsAssigned<Bleed>();
            if (!targetHasBleed) return false;
            
            Bleed bleed = caster.Target.Resources.Get<Bleed>();
            return base.CanExecute(caster) && bleed.Count > 0;
        }

        private void OnDoneCasting()
        {
            targetChar.Resources.Get<Bleed>().AddStack(-1);

            targetChar.TryDamage(casterChar, 20f);
            
            casterChar.Animator.PlayFlipBook("attack");
            casterChar.Animator.BackToPosition();
            
            targetChar.StatusEffects.Add(new LeechStatusEffect(casterChar, new(4, 10), new(3, 8f), 5f));
        }
    }
}