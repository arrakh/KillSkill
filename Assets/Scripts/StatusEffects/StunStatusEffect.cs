namespace StatusEffects
{
    public class StunStatusEffect : StatusEffect, IPreventAbilityCasting
    {
        public override string DisplayName => "Stunned";

        public StunStatusEffect(float duration) : base(duration)
        {
        }
    }
}