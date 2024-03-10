using System;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Modules;
using KillSkill.UI.Game.Events;
using TMPro;
using UnityEngine;

namespace KillSkill.UI.Game
{
    //todo: create module
    public class TimerView : View, IQueryProvider<BattleTimerQuery>
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private BattleSequenceModule sequenceModule; //todo: will need to be from module later

        private float currentSeconds = 0;

        //should be in module
        private void Start()
        {
            GlobalEvents.RegisterQuery(this);
        }

        private void OnDestroy()
        {
            GlobalEvents.UnregisterQuery<BattleTimerQuery>();
        }

        private void Update()
        {
            
            if (!sequenceModule.IsBattlePaused) currentSeconds += Time.deltaTime;
            
            int hours = (int)(currentSeconds / 3600);
            int minutes = (int)(currentSeconds / 60) % 60;
            int seconds = (int)(currentSeconds % 60);
            int milliseconds = (int)((currentSeconds - Mathf.Floor(currentSeconds)) * 100);


            if(hours > 0)
            {
                timerText.text = $"{hours:00}:{minutes:00}:{seconds:00}s";
            }
            else if(minutes > 0)
            {
                timerText.text = $"{minutes:00}:{seconds:00}s";
            }
            else
            {
                timerText.text = $"{seconds:00}.{milliseconds:00}s";
            }
        }

        public BattleTimerQuery OnQuery()
        {
            return new BattleTimerQuery() {timeInSeconds = currentSeconds};
        }
    }
}