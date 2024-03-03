using System;
using System.Collections.Generic;
using CharacterResources;
using KillSkill.Characters;

namespace KillSkill.Battle
{
    public class BattleResultState
    {
        public readonly bool playerWon;
        public readonly IReadOnlyDictionary<Type, ICharacterResource> playerResources;
        public readonly IReadOnlyDictionary<Type, ICharacterResource> enemyResources;
        public readonly float battleDurationSeconds;

        public BattleResultState(bool playerWon, IReadOnlyDictionary<Type, ICharacterResource> playerResources, IReadOnlyDictionary<Type, ICharacterResource> enemyResources, float battleDurationSeconds)
        {
            this.playerWon = playerWon;
            this.playerResources = playerResources;
            this.enemyResources = enemyResources;
            this.battleDurationSeconds = battleDurationSeconds;
        }
    }
}