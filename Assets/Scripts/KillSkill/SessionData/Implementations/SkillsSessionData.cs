using System;
using System.Collections.Generic;
using Arr.EventsSystem;
using KillSkill.SessionData.Events;
using KillSkill.Skills;
using KillSkill.Skills.Implementations.Fighter;
using SessionData.Implementations;
using Skills;
using UnityEngine;
using UnityEngine.Events;

namespace KillSkill.SessionData.Implementations
{
    public class SkillsSessionData : ISessionData
    {
        private List<Skill> loadout = new()
        {
            new SlashSkill(),
            new VigorSkill(),
            new QuickBlockSkill(),
            null,
            null,
            null,
        };
        
        private Dictionary<Type, Skill> loadoutByType = new();

        private HashSet<Type> ownedSkills = new()
        {
            typeof(SlashSkill),
            typeof(VigorSkill),
            typeof(QuickBlockSkill),
        };

        public IReadOnlyCollection<Skill> Loadout => loadout;

        public int SlotCount => loadout.Count;
        
        public bool IsEquipped(Skill skill) => loadoutByType.ContainsKey(skill.GetType());
        public bool Owns(Skill skill) => ownedSkills.Contains(skill.GetType());
        public bool Owns<T>() where T : Skill => ownedSkills.Contains(typeof(T));

        public bool Add(Type skillType)
        {
            if (!typeof(Skill).IsAssignableFrom(skillType))
            {
                Debug.LogError($"Trying to add {skillType} but it is NOT a Skill");
                return false;
            }

            if (ownedSkills.Contains(skillType))
            {
                Debug.LogError($"Trying to add {skillType} but it is already owned!");
                return false;
            }

            ownedSkills.Add(skillType);
            GlobalEvents.Fire(new SkillsUpdatedEvent(this));
            return true;
        }

        public bool HasEmptySlot()
        {
            foreach (var skill in loadout)
                if (skill == null) return true;

            return false;
        }

        public void Equip(Skill skill, int index)
        {
            int finalIndex = index;
            if (index >= loadout.Count) finalIndex = GetFirstUnoccupiedIndex();

            loadout[finalIndex] = skill;
            loadoutByType[skill.GetType()] = skill;
            GlobalEvents.Fire(new SkillsUpdatedEvent(this));
        }

        public void Unequip(Skill skill)
        {
            var index = GetSkillIndex(skill.GetType());
            loadout[index] = null;
            loadoutByType.Remove(skill.GetType());
            GlobalEvents.Fire(new SkillsUpdatedEvent(this));
        }

        private int GetSkillIndex(Type skillType)
        {
            for (int i = 0; i < loadout.Count; i++)
            {
                var skill = loadout[i];
                if (skill == null) continue;
                if (skill.GetType() == skillType) return i;
            }

            throw new Exception($"Trying to Get skill index for {skillType} but cannot find!");
        }

        public void AddSlot(int count)
        {
            for (int i = 0; i < count; i++)
                loadout.Add(null);
            GlobalEvents.Fire(new SkillsUpdatedEvent(this));
        }

        public void RemoveSlot(int count)
        {
            for (int i = 0; i < count; i++)
                loadout.RemoveAt(loadout.Count - 1);
            GlobalEvents.Fire(new SkillsUpdatedEvent(this));
        }

        private int GetFirstUnoccupiedIndex()
        {
            for (var index = 0; index < loadout.Count; index++)
            {
                var skill = loadout[index];
                if (skill == null) return index;
            }

            return loadout.Count - 1;
        }


        public void OnLoad()
        {
            foreach (var skill in loadout)
            {
                if (skill == null) continue;
                loadoutByType[skill.GetType()] = skill;
            }
        }

        public void OnUnload()
        {
            
        }
    }
}