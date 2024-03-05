using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface IModifyIncomingHeal
    {
        public void ModifyHeal(Character healer, Character target, ref double heal);
    }
    
    public interface IModifyHealingDealt
    {
        public void ModifyHeal(Character healer, Character target, ref double heal);
    }
}