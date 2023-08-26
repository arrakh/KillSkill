using Actors;

namespace StatusEffects
{
    public interface IModifyHealStatusEffect
    {
        public void ModifyDamage(Character healer, ref float heal);
    }
}