using System;
using CharacterResources;
using KillSkill.Characters;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations;
using KillSkill.UI;
using KillSkill.UI.Game;
using UI;
using UnityEngine;

namespace KillSkill.CharacterResources.Implementations
{
    public class StanceStagger : ICharacterResource, IModifyIncomingDamage, IResourceDisplay<ResourceBarDisplay>
    {
        private const float STAGGER_TIME = 6f;
        
        private ICharacter owner;
        private double currentValue;
        
        public event Action<ResourceBarDisplay> OnUpdateDisplay;
        public ResourceBarDisplay DisplayData { get; }

        public StanceStagger(ICharacter owner, double maxThreshold)
        {
            this.owner = owner;
            currentValue = maxThreshold;

            DisplayData = new()
            {
                value = currentValue,
                min = 0,
                max = maxThreshold,
                barColor = new Color(242/255f, 179/255f, 31/255f)
            };
        }

        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            currentValue -= damage;
            DisplayData.value = currentValue;

            if (currentValue > 0)
            {
                OnUpdateDisplay?.Invoke(DisplayData);
                return;
            }
            
            owner.Resources.Unassign<StanceStagger>();
            owner.StatusEffects.Add(new StaggeredStatusEffect(STAGGER_TIME));
            if (owner.StatusEffects.Has<StancingStatusEffect>()) 
                owner.StatusEffects.Remove<StancingStatusEffect>();
        }

        public static string StandardDescription() =>
            $"When this resource hits 0, the user is instantly <u>Staggered</u> for {STAGGER_TIME} seconds";
    }
}