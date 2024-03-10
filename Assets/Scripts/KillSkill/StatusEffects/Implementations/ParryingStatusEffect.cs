using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class ParryingStatusEffect : TimedStatusEffect, IModifyIncomingDamage
    {
        private Character character;
        private Action<bool> onResult;
        private float damageReduceMultiplier;
        private bool success = false;
        
        public ParryingStatusEffect(Character character, Action<bool> onResult, float damageReduceMultiplier, float duration) : base(duration)
        {
            this.character = character;
            this.onResult = onResult;
            this.damageReduceMultiplier = damageReduceMultiplier;
        }


        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-parrying"),
            name = "Parrying",
            description = StandardDescription()
        };

        public static string StandardDescription() =>
            "While active, parrying is a SUCCESS when the user is damaged. Parrying is FAILED when the timer runs out";

        public override void OnRemoved(Character target)
        {
            onResult.Invoke(success);
            base.OnRemoved(target);
        }

        public void ModifyDamage(Character damager, Character target, ref double damage)
        {
            success = true;
            damage -= damage * damageReduceMultiplier;
            character.StatusEffects.TryRemove<ParryingStatusEffect>();
        }
    }
}