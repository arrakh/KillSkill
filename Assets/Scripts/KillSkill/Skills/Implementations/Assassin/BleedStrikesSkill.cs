using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class BleedStrikesSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 3f;

        [Configurable] private Range damage = new(1f, 3f);
        [Configurable] private int attackCount = 3;
        [Configurable] private int bleedCountPerAttack  = 1;
        [Configurable] private float castDuration = 2f;

        private ICharacter targetChar;
        private ICharacter casterChar;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Bleed Strikes",
            description = $"Attack {attackCount} times, each dealing {damage} damage granting {bleedCountPerAttack} <u>Bleed</u>",
            icon = SpriteDatabase.Get("skill-bleed-strikes")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            targetChar = target;
            casterChar = caster;
            
            var repeatCastDuration = castDuration / attackCount;
            caster.StatusEffects.Add(new RepeatCastingStatusEffect(repeatCastDuration, attackCount, OnCastCycle));
        }

        private void OnCastCycle(int index)
        {
            if(!targetChar.TryDamage(casterChar, damage.GetRandom())) return;
            
            if (targetChar.Resources.TryGet(out Bleed bleed)) bleed.AddStack(bleedCountPerAttack);
            else targetChar.Resources.Assign(new Bleed(targetChar, bleedCountPerAttack));
        }
    }
}