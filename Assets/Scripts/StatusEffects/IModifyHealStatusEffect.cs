using Actors;

namespace StatusEffects
{
    public interface IModifyHealStatusEffect
    {
        public void ModifyHeal(Character healer, ref double heal);
    }
}