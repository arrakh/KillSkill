using UnityEngine.TextCore.Text;

namespace StatusEffects
{
    public interface IModifyCastingSpeed
    {
        public void ModifyCastingSpeed(Character character, ref float castingSpeed);
    }
}