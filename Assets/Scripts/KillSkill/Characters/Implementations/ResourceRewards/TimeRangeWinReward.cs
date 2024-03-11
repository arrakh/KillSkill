using System;
using KillSkill.Battle;

namespace KillSkill.Characters.Implementations.ResourceRewards
{
    public class TimeRangeWinReward : IResourceReward
    {
        private string id;
        private int amount;
        private float minSecond, maxSecond;
        private string customMessage;

        public TimeRangeWinReward(string id, int amount, float maxSecond)
        {
            this.id = id;
            this.amount = amount;
            minSecond = 0;
            this.maxSecond = maxSecond;
        }

        public TimeRangeWinReward(string id, int amount, float minSecond, float maxSecond, string customMessage = "")
        {
            this.id = id;
            this.amount = amount;
            this.minSecond = minSecond;
            this.maxSecond = maxSecond;
            this.customMessage = customMessage;
        }

        public bool TryCalculateReward(BattleResultState state, out BattleReward reward)
        {
            reward = default;

            if (!state.playerWon) return false;
            
            var time = state.battleDurationSeconds;
            if (time > maxSecond || time < minSecond) return false;

            var msg = $"Victory within {FormatTime(maxSecond)}";
            var hasCustomMsg = !string.IsNullOrEmpty(customMessage);
            reward = new(hasCustomMsg ? customMessage : msg, id, amount);
            return true;
        }

        private string FormatTime(float time)
        {
            var span = TimeSpan.FromSeconds(time);
            if (span.TotalHours >= 1) return $"{span.TotalHours:0}h {span.Minutes:0}m";
            if (span.TotalMinutes >= 1) return $"{span.TotalMinutes:0}m {span.Seconds:0}s";
            return $"{span.TotalSeconds:0}s";
        }
    }
}