namespace KillSkill.Battle
{
    public class BattleReward
    {
        public readonly string resultText;
        public readonly string resourceId;
        public readonly double resourceAmount;

        public BattleReward(string resultText, string resourceId, double resourceAmount)
        {
            this.resultText = resultText;
            this.resourceId = resourceId;
            this.resourceAmount = resourceAmount;
        }
    }
}