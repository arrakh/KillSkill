using KillSkill.Battle;

namespace KillSkill.Characters.Implementations.ResourceRewards
{
    public class CustomMessageReward : IResourceReward
    {
        private string id;
        private int amount;
        private string message;

        public CustomMessageReward(string id, int amount, string message)
        {
            this.id = id;
            this.amount = amount;
            this.message = message;
        }

        bool IResourceReward.TryCalculateReward(BattleResultState state, out BattleReward reward)
        {
            reward = new(message, id, amount);
            return state.playerWon;
        }
    }
}