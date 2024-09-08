using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Modules.Battle.Events;
using KillSkill.Modules.Network.Events;
using KillSkill.Network.Messages.Battle;
using KillSkill.UI.Battle.Events;
using KillSkill.UI.Game;

namespace KillSkill.UI.Battle.Modules
{
    public class TimerViewModule : ViewModule<TimerView>, 
        IQueryProvider<BattleTimerQuery>,
        IEventListener<NetMessageEvent<BattlePauseNetMessage>>
    {
        
        
        //This shouldn't take data from view, should be handled here, but for now this is fine.
        public BattleTimerQuery OnQuery()
        {
            return new BattleTimerQuery() {timeInSeconds = view.CurrentSeconds};
        }

        public void OnEvent(NetMessageEvent<BattlePauseNetMessage> message)
        {
            view.SetPause(message.message.IsPaused);
        }
    }
}