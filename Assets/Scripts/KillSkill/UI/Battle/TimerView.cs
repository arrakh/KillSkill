using System;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Modules;
using TMPro;
using UnityEngine;

namespace KillSkill.UI.Game
{
    //todo: create module
    public class TimerView : View
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private float currentSeconds = 0;
        private bool pause = true;

        public float CurrentSeconds => currentSeconds;

        public void SetPause(bool on) => pause = on;

        private void Update()
        {
            
            if (!pause) currentSeconds += Time.deltaTime;
            
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
    }
}