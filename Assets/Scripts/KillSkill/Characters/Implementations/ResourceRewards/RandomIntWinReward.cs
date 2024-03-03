using KillSkill.Battle;

namespace KillSkill.Characters.Implementations.ResourceRewards
{
    public class RandomIntWinReward : IResourceReward
    {
        private string id;
        private int min, max;

        public RandomIntWinReward(string id, int min, int max)
        {
            this.id = id;
            this.min = min;
            this.max = max;
        }

        string IResourceReward.Id => id;

        double IResourceReward.CalculateReward(BattleResultState state)
        {
            if (!state.playerWon) return 0;
            return UnityEngine.Random.Range(min, max);
        }
    }
}