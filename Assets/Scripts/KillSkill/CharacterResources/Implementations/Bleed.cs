using System;
using CharacterResources;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.UI.Game;

namespace KillSkill.CharacterResources.Implementations
{
    public class Bleed : ICharacterResource, IUpdatableCharacterResource, IResourceDisplay<ResourceFillCounterDisplay>
    {
        public Bleed(ICharacter owner, int stackCount)
        {
            this.owner = owner;
            this.stackCount = stackCount;
            progress = durationPerStack;

            DisplayData = new()
            {
                value = this.stackCount,
                fillValue = 1f,
                icon = SpriteDatabase.Get("resources-bleed")
            };
        }

        public event Action<ResourceFillCounterDisplay> OnUpdateDisplay;
        public ResourceFillCounterDisplay DisplayData { get; }

        private ICharacter owner;
        private float durationPerStack = 1f;
        private float progress;
        private int stackCount;
        private float damagePerStack = 5f;

        public void AddStack(int delta) => stackCount += delta;
        public void AddDurationPerStack(float delta) => durationPerStack += delta;
        public void AddDamagePerStack(float delta) => damagePerStack += delta;
        
        public void OnUpdate(float deltaTime)
        {
            progress -= deltaTime;
            EvaluateBleed();

            if (stackCount <= 0)
            {
                owner.Resources.Unassign<Bleed>();
                return;
            }
            
            DisplayData.value = stackCount;
            DisplayData.fillValue = progress / durationPerStack;
            OnUpdateDisplay?.Invoke(DisplayData);
        }

        private void EvaluateBleed()
        {
            if (progress > 0) return;
            progress += durationPerStack;
            stackCount -= 1;
            owner.TryDamage(owner, damagePerStack);
        }
    }
}