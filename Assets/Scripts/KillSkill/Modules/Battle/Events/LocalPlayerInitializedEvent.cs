using KillSkill.Battle;
using KillSkill.Characters;

namespace KillSkill.Modules.Battle.Events
{
    public struct LocalPlayerInitializedEvent
    {
        public ICharacter localPlayer;

        public LocalPlayerInitializedEvent(ICharacter localPlayer)
        {
            this.localPlayer = localPlayer;
        }
    }
}