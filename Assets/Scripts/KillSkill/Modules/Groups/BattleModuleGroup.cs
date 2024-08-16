using Arr.ModulesSystem;
using KillSkill.Modules.Battle;
using KillSkill.Modules.Network;
using KillSkill.UI.Battle.Modules;

namespace KillSkill.Modules.Groups
{
    public class BattleModuleGroup : ModuleGroup
    {
        public override IModule[] Modules => new IModule[]
        {
            new BattleControllerModule(),
            new BattleFactoryModule(),
            new PostProcessModule(),

            new CountdownViewModule(),
            new ResultViewModule(),
            new PauseViewModule(),
            new TimerViewModule(),
            new GameSkillIndicatorsViewModule(),
            
            //MUST BE AT THE END
            new CommitModule()
        };
    }
}