using System;
using CharacterResources;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.UI;
using KillSkill.UI.Battle;
using KillSkill.UI.Game;
using KillSkill.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KillSkill.CharacterResources.Implementations
{
    public class Bleed : ICharacterResource, IStackable, IModifyIncomingDamage, IResourceDisplay<ResourceCounterDisplay>
    {
        [Configurable] private float consumeChancePercent = 30f;
        [Configurable] private float damageOnConsume = 10f;
        
        public Bleed(ICharacter owner, int stackCount)
        {
            this.owner = owner;
            this.stackCount = stackCount;

            DisplayData = new()
            {
                value = this.stackCount,
                icon = SpriteDatabase.Get("resources-bleed")
            };
        }

        public event Action<ResourceCounterDisplay> OnUpdateDisplay;
        public ResourceCounterDisplay DisplayData { get; }

        private ICharacter owner;
        private int stackCount;
        private float damagePerStack = 5f;

        public void AddStack(int count) => stackCount += count;

        public int Count => stackCount;
        
        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            var consumed = Random.Range(0f, 100f) < consumeChancePercent;
            if (!consumed) return;

            if (!target.TryDamage(owner, damageOnConsume)) return;
            
            target.VisualEffects.Spawn("bleed-splat", target.Position);
            stackCount--;

            if (stackCount <= 0)
            {
                owner.Resources.Unassign<Bleed>();
                return;
            }
            
            Debug.Log("Soemthing");
            
            DisplayData.value = stackCount;
            OnUpdateDisplay?.Invoke(DisplayData);
        }
    }
}