using KillSkill.Battle;

namespace KillSkill.Characters.Implementations.ResourceRewards
{
    public class RandomIntReward : IResourceReward
    {
        private string id;
        private int minWin, maxWin;
        private int minLose, maxLose;

        public RandomIntReward(string id, int minWin, int maxWin, int minLose, int maxLose)
        {
            this.id = id;
            this.minWin = minWin;
            this.maxWin = maxWin;
            this.minLose = minLose;
            this.maxLose = maxLose;
        }

        bool IResourceReward.TryCalculateReward(BattleResultState state, out BattleReward reward)
        {
            var value = state.playerWon
                ? UnityEngine.Random.Range(minWin, maxWin)
                : UnityEngine.Random.Range(minLose, maxLose);

            var resultText = state.playerWon ? "Victory" : "Defeat";

            reward = new(resultText, id, value);

            return true;
        }
    }
}