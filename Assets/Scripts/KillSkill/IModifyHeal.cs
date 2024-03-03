using KillSkill.Characters;

namespace KillSkill
{
    public interface IModifyHeal
    {
        public void ModifyHeal(Character healer, ref double heal);
    }
}