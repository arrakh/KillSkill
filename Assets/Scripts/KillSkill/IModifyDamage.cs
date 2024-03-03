using KillSkill.Characters;

namespace KillSkill
{
    public interface IModifyDamage
    {
        public void ModifyDamage(Character damager, ref double damage);
    }
}