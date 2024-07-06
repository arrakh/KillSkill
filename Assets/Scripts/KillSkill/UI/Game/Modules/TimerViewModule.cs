using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Modules.Battle.Events;
using KillSkill.UI.Game.Events;

namespace KillSkill.UI.Game.Modules
{
    public class TimerViewModule : ViewModule<TimerView>, 
        IQueryProvider<BattleTimerQuery>,
        IEventListener<BattlePauseEvent>
    {
        
        
        //This shouldn't take data from view, should be handled here, but for now this is fine.
        public BattleTimerQuery OnQuery()
        {
            return new BattleTimerQuery() {timeInSeconds = view.CurrentSeconds};
        }

        public void OnEvent(BattlePauseEvent data)
        {
            view.SetPause(data.paused);
        }
    }
}