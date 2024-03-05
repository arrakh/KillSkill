using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface IModifyIncomingDamage
    {
        public void ModifyDamage(Character damager, Character target, ref double damage);
    }
    
    public interface IModifyDamageDealt
    {
        public void ModifyDamage(Character damager, Character target, ref double damage);
    }
}