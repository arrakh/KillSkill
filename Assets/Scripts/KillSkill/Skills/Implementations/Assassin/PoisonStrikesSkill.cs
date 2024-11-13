using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class PoisonStrikesSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 3f;

        [Configurable] private Range damage = new(1f, 3f);
        [Configurable] private int attackCount = 3;
        [Configurable] private int poisonCountPerAttack  = 1;
        [Configurable] private float castDuration = 2f;

        private ICharacter targetChar;
        private ICharacter casterChar;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Poison Strikes",
            description = $"Attack {attackCount} times, each dealing {damage} damage granting {poisonCountPerAttack} <u>Poison</u>",
            icon = SpriteDatabase.Get("skill-poison-strikes")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN, 5);

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
            
            if (targetChar.Resources.TryGet(out Poison poison)) poison.AddStack(poisonCountPerAttack);
            else targetChar.Resources.Assign(new Poison(targetChar, poisonCountPerAttack));
        }
    }
}