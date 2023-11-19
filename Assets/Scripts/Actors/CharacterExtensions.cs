using CharacterResources.Implementations;

namespace Actors
{
    public static class CharacterExtensions
    {
        public static bool TryDamage(this Character character, Character damager, double damage)
            => character.Resources.TryAdd<Health>(-damage, damager);
        
        public static bool TryHeal(this Character character, Character healer, double heal)
            => character.Resources.TryAdd<Health>(heal, healer);
    }
}