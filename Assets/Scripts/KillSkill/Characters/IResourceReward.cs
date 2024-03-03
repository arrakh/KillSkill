using KillSkill.Battle;

namespace KillSkill.Characters
{
    public interface IResourceReward
    {
        public string Id { get; }
        public double CalculateReward(BattleResultState state);
    }
}