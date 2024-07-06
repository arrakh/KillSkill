using System;
using System.Collections.Generic;
using Arr;
using Arr.Utils;
using CharacterResources;
using DefaultNamespace;
using KillSkill.CharacterResources;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Skills;
using Skills;
using StatusEffects;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    //todo: CHARACTER SHOULD BE INTERFACED TO ICHARACTER
    public interface ICharacter 
    {
        public bool IsAlive { get; }

        public int Uid { get; }

        public event Action<ICharacter> onDeath;

        public ICharacter Target { get; }
        
        public IStatusEffectsHandler StatusEffects { get; }
        
        public ICharacterResourcesHandler Resources { get; }

        public ICharacterAnimator Animator { get; }
        public ICharacterSkillHandler Skills { get; }
        
        public IVisualEffectsHandler VisualEffects { get; }
        public ICharacterFactory CharacterFactory { get; }

        public PersistentEventTemplate<ICharacter> OnInitialize { get; }
        
        public Vector3 Position { get; set; }
        
        public GameObject GameObject { get; }
        
        public Type MainResource { get; }

        public void Initialize(ICharacterData characterData, ICharacterFactory factory,
            IVisualEffectsHandler vfx);

        public void SetBattlePaused(bool paused);

        public void SetTarget(ICharacter newTarget);

        public void Kill();
    }
}