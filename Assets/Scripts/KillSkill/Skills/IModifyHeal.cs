using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface IModifyIncomingHeal
    {
        public void ModifyHeal(ICharacter healer, ICharacter target, ref double heal);
    }
    
    public interface IModifyHealingDealt
    {
        public void ModifyHeal(ICharacter healer, ICharacter target, ref double heal);
    }
}