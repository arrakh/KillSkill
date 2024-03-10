using KillSkill.Battle;

namespace KillSkill.Characters
{
    public interface IResourceReward
    {
        public bool TryCalculateReward(BattleResultState state, out BattleReward reward);
    }
}