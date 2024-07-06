using System.Collections.Generic;
using FlipBooks;
using KillSkill.Skills;

namespace KillSkill.Characters
{
    public interface ICharacterData
    {
        //todo: Should definitely be a Stats kinda thing, not here
        public string Id { get; }
        
        public float Health { get; }
        
        public Skill[] Skills { get; }
    }
}