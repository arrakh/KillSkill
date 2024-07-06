using System.Collections.Generic;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using Skills;
using StatusEffects;

namespace KillSkill.Skills.Implementations.Fighter
{
    public class CleaveSkill : Skill
    {
        protected override float CooldownTime => 10f;
        
        private const float DAMAGE = 25f;

        public override SkillMetadata Metadata => new()
        {
            name = "Cleave",
            description = $"Instantly deal {DAMAGE} damage",
            icon = SpriteDatabase.Get("skill-cleave")
        };

        public override CatalogEntry CatalogEntry => new()
        {
            archetypeId = Archetypes.FIGHTER,
            order = 2, resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 50}
            }
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            target.TryDamage(caster, DAMAGE);
            
            Tween forward = caster.Animator.Visual.DOMoveX(caster.Animator.Visual.position.x + 2f, 0.15f)
                .SetEase(Ease.OutQuart).OnComplete(() => {caster.Animator.BackToPosition();});
            
            caster.Animator.AddMovementTweens(forward);
            caster.Animator.PlayFlipBook("attack");
        }
    }
}