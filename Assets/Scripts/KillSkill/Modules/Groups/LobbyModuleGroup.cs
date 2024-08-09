using Arr.ModulesSystem;
using KillSkill.Modules.Lobby;

namespace KillSkill.Modules.Groups
{
    public class LobbyModuleGroup : ModuleGroup
    {
        public override IModule[] Modules => new IModule[]
        {
             new ArenaViewModule(),
             new SkillsManagerViewModule(),
             new MultiplayerViewModule(),
             new NavigationViewModule(),
        };
    }
}