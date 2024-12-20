﻿using System.Collections.Generic;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.HeavyKnight
{
    public class CancelStanceSkill : Skill
    {
        protected override float CooldownTime => 0.5f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-cancel-stance"),
            name = "Cancel Stance",
            description = "Safely cancels <u>Stancing</u>. Can only be activated when the user is <u>Stancing</u>.",
            extraDescription = $"- <u>Stancing</u>:\n{StancingStatusEffect.StandardDescription()}\n\n" +
                               $"- <u>Stagger</u>:\n{StaggeredStatusEffect.StandardDescription()}"
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 7, archetypeId = Archetypes.WARRIOR,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 100},
                {GameResources.MEDALS, 1},
            },
        };

        public override bool CanExecute(ICharacter caster)
            => base.CanExecute(caster) && caster.Resources.IsAssigned<StanceStagger>();

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.Resources.Unassign<StanceStagger>();
            caster.StatusEffects.Remove<StancingStatusEffect>();
            caster.Animator.BackToPosition();
            caster.Animator.PlayFlipBook("idle");
        }
    }
}