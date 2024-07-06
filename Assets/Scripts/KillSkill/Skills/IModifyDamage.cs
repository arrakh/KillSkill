using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface IModifyIncomingDamage
    {
        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage);
    }
    
    public interface IModifyDamageDealt
    {
        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage);
    }
}