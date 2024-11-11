using System;
using CharacterResources;
using KillSkill.CharacterResources;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using KillSkill.Utility;
using StatusEffects;
using Unity.VisualScripting;

namespace KillSkill.StatusEffects.Implementations
{
    public class InfuseAttackStatusEffect<T> : TimedStatusEffect, IModifyDamageDealt where T : ICharacterResource, IStackable
    {
        [Configurable] private float successChancePercent = 60f;
        [Configurable] private bool grantToTarget = true;
        
        private int amount = 1;

        private T localInstance;
        
        public InfuseAttackStatusEffect(float duration, int amount) : base(duration)
        {
            this.amount = amount;
            localInstance = Activator.CreateInstance<T>();
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-infuse-attack"),
            name = $"Infuse Attack: {nameof(T)}", //nameof BADD, should use instance
            description = StandardDescription<T>(successChancePercent, grantToTarget, amount)
        };

        private string StandardDescription<T>(float successChancePercent, bool grantToTarget, int amount)
            where T : ICharacterResource
            => $"When attacking, {successChancePercent}% chance to grant {amount} {nameof(T)} to {(grantToTarget ? "Target" : "Owner")}";

        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            var success = UnityEngine.Random.Range(0f, 100f) < successChancePercent;
            if (!success) return;

            var toGrant = grantToTarget ? target : damager;
            
            if (toGrant.Resources.TryGet(out T instance))
            {
                instance.AddStack(amount);
                return;
            }
            
            localInstance = Activator.CreateInstance<T>();
            localInstance.AddStack(amount);
            toGrant.Resources.Assign(localInstance);
        }
    }
}