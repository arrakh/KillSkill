﻿using Arr.ModulesSystem;
using KillSkill.Modules.Network;
using KillSkill.Modules.VisualEffects;
using KillSkill.SessionData;
using KillSkill.UI;

namespace KillSkill.Modules.Groups
{
    public class MasterModuleGroup : ModuleGroup
    {
        public override IModule[] Modules => new IModule[]
        {
            new SessionDataModule(),
            
            new NetworkModule(),
            new NetworkPartyModule(),
            
            new VisualEffectModule(),
            new CameraControlModule(),
            new UnityEventSystemModule(),
            new TooltipViewModule(),
            new SkillSessionModule()
        };
    }
}