using System;
using System.Collections.Generic;
using CharacterResources;
using KillSkill.Characters;

namespace KillSkill.Battle
{
    public class BattleResultState
    {
        public readonly bool playerWon;
        public readonly float battleDurationSeconds;
        public IReadOnlyList<ICharacter> characters;

        public BattleResultState(bool playerWon, float battleDurationSeconds, IReadOnlyList<ICharacter> characters)
        {
            this.playerWon = playerWon;
            this.battleDurationSeconds = battleDurationSeconds;
            this.characters = characters;
        }
    }
}