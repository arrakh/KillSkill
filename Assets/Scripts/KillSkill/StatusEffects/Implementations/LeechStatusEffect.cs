using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations.Core;
using KillSkill.Utility;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class LeechStatusEffect : TimedStatusEffect
    {
        private Range damage = new(40, 100f);
        private Range heal = new(30, 80);
        private float tickDuration = 0.5f;
        private ICharacter owner;

        private float currentDuration = 0f;
        
        public LeechStatusEffect(ICharacter owner, Range damage, Range heal, float duration) : base(duration)
        {
            this.owner = owner;
            this.damage = damage;
            this.heal = heal;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-empowered"),
            name = "Leech",
            description = StandardDescription(damage, heal)
        };
        
        public static string StandardDescription(Range damage, Range heal) => $"When removed, the target is attacked for {damage} and owner is healed for {heal}";

        public override void OnUpdate(ICharacter target, float deltaTime)
        {
            currentDuration += deltaTime;
            if (currentDuration < tickDuration) return;
            
            target.TryDamage(owner, damage.GetRandomRounded());
            owner.TryHeal(owner, heal.GetRandomRounded());
            
            currentDuration -= tickDuration;
        }
    }
}