using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class BarrageOfStrikesSkill : Skill, IGlobalCooldownSkill
    {
        
        [Configurable] private Range damage = new (4f, 10f);
        [Configurable] private float stanceDuration = 1.2f;
        [Configurable] private float stanceStaggerLimit = 25f;
        [Configurable] private float durationPerAttack = 0.15f;

        private int attackCount;
        private ICharacter casterChar;
        private ICharacter targetChar;
        
        protected override float CooldownTime => 4f;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Barrage of Strikes",
            description = $"Enter </u>Stancing</u> for {stanceDuration} seconds, then attack as many as target's total Bleed and Poison for {damage} damage each",
            extraDescription = $"- <u>Stancing</u>: {StancingStatusEffect.StandardDescription(stanceStaggerLimit)}",
            icon = SpriteDatabase.Get("skill-barrage-of-strikes")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN, 7);

        public override bool CanExecute(ICharacter caster)
            => base.CanExecute(caster) && GetAttackCount(caster.Target) > 0;

        int GetAttackCount(ICharacter target)
        {
            int count = 0;
            if (target.Resources.TryGet(out Bleed bleed)) count += bleed.Count;
            if (target.Resources.TryGet(out Poison poison)) count += poison.Count;
            return count;
        }

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            attackCount = GetAttackCount(target);
            caster.StatusEffects.Add(new StancingStatusEffect(OnDoneStancing, stanceStaggerLimit, stanceDuration));
        }

        private void OnDoneStancing()
        {
            var totalDuration = durationPerAttack * attackCount;
            casterChar.StatusEffects.Add(new RepeatCastingStatusEffect(totalDuration, attackCount, OnAttackCycle));
        }

        private void OnAttackCycle(int index)
        {
            casterChar.Animator.PlayFlipBook("attack");

            targetChar.TryDamage(casterChar, damage.GetRandomRounded());
            if (index < attackCount - 1) return;
            
            if (targetChar.Resources.IsAssigned<Bleed>()) targetChar.Resources.Unassign<Bleed>();
            if (targetChar.Resources.IsAssigned<Poison>()) targetChar.Resources.Unassign<Poison>();
        }
    }
}