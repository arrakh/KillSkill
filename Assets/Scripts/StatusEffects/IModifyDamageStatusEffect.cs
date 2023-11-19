using Actors;

namespace StatusEffects
{
    public interface IModifyDamageStatusEffect
    {
        public void ModifyDamage(Character damager, ref double damage);
    }
}