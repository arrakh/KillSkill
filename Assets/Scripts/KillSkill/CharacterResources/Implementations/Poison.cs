using System;
using CharacterResources;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.UI.Battle;
using KillSkill.Utility;

namespace KillSkill.CharacterResources.Implementations
{
    public class Poison : ICharacterResource, IStackable, IModifyIncomingDamage, IResourceDisplay<ResourceCounterDisplay>
    {
        [Configurable] private float consumeChancePercent = 20f;
        [Configurable] private float multiplierPerCount = 0.05f;
        
        public Poison(ICharacter owner, int stackCount)
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

        public void AddStack(int delta) => stackCount += delta;
        
        public int Count => stackCount;

        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            var consumed = UnityEngine.Random.Range(0f, 100f) < consumeChancePercent;
            if (!consumed) return;

            var mult = 1f + multiplierPerCount * stackCount;
            damage *= mult;
            
            target.VisualEffects.Spawn("poison-splat", target.Position);
            stackCount--;
            DisplayData.value = stackCount;
            OnUpdateDisplay?.Invoke(DisplayData);
        }
    }
}