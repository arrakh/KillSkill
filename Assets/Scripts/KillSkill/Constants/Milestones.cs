namespace KillSkill.Constants
{
    public static class Milestones
    {
        public const string HAS_DEFEATED = "has-defeated";
        public static string HasDefeated(string enemyId) => $"{HAS_DEFEATED}-{enemyId}";

    }
}