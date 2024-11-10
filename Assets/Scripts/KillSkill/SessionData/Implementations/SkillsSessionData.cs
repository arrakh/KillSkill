using System;
using System.Collections.Generic;
using System.Linq;
using Arr.EventsSystem;
using KillSkill.Constants;
using KillSkill.Network;
using KillSkill.SessionData.Events;
using KillSkill.Skills;
using KillSkill.Skills.Implementations.Warrior;
using Unity.Netcode;
using UnityEngine;
using KillSkill.Utility;

namespace KillSkill.SessionData.Implementations
{
    public class SkillsSessionData : ISessionData, ILoadableSessionData, INetCodeSerializable
    {
        //todo: move default loadout to config
        private List<Type> loadout = new()
        {
            typeof(SlashSkill),
            typeof(EmptySkill)
        };
        
        private HashSet<Type> loadoutByType = new();

        private HashSet<Type> ownedSkills = new()
        {
            typeof(SlashSkill),
        };

        public IReadOnlyCollection<Type> Loadout => loadout;

        public int SlotCount => loadout.Count;
        
        public bool IsEquipped(Skill skill) => loadoutByType.Contains(skill.GetType());
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
            GlobalEvents.Fire(new SessionUpdatedEvent<SkillsSessionData>(this));
            return true;
        }

        public bool HasEmptySlot()
        {
            foreach (var skill in loadout)
                if (skill.IsEmpty()) return true;

            return false;
        }

        public void Equip(Skill skill, int index)
        {
            if (IsEquipped(skill)) Unequip(skill);
            
            int finalIndex = index;
            if (index >= loadout.Count) finalIndex = GetFirstUnoccupiedIndex();

            var type = skill.GetType();
            loadout[finalIndex] = type;
            loadoutByType.Add(type);
            GlobalEvents.Fire(new SessionUpdatedEvent<SkillsSessionData>(this));
        }

        public void Unequip(Skill skill)
        {
            var index = GetSkillIndex(skill.GetType());
            Unequip(index);
        }

        public void Unequip(int skillIndex)
        {
            if (skillIndex > loadout.Count)
                throw new Exception($"Cannot unequip index {skillIndex}, loadout only has {loadout.Count} slots!");
            
            var skill = loadout[skillIndex];
            if (skill.IsEmpty()) return;
            loadoutByType.Remove(skill);
            loadout[skillIndex] = null;
            GlobalEvents.Fire(new SessionUpdatedEvent<SkillsSessionData>(this));
        }

        private int GetSkillIndex(Type skillType)
        {
            for (int i = 0; i < loadout.Count; i++)
            {
                var skill = loadout[i];
                if (skill.IsEmpty()) continue;
                if (skill == skillType) return i;
            }

            throw new Exception($"Trying to Get skill index for {skillType} but cannot find!");
        }

        public void AddSlot(int count)
        {
            for (int i = 0; i < count; i++)
                loadout.Add(null);
            GlobalEvents.Fire(new SessionUpdatedEvent<SkillsSessionData>(this));
        }

        public void RemoveSlot(int count)
        {
            for (int i = 0; i < count; i++)
                loadout.RemoveAt(loadout.Count - 1);
            GlobalEvents.Fire(new SessionUpdatedEvent<SkillsSessionData>(this));
        }

        private int GetFirstUnoccupiedIndex()
        {
            for (var index = 0; index < loadout.Count; index++)
            {
                var skill = loadout[index];
                if (skill.IsEmpty()) return index;
            }

            return loadout.Count - 1;
        }


        public void OnLoad()
        {
            SkillTypeMapper.Initialize();
            Initialize();
        }

        private void Initialize()
        {
            foreach (var skill in loadout)
            {
                if (skill.IsEmpty()) continue;
                loadoutByType.Add(skill);
            }
        }

        public void OnUnload()
        {
            
        }

        //todo: move into a config
        public Dictionary<string,double> GetSlotCost()
        {
            return SlotCount switch
            {
                1 or 2 or 3 or 4 => new Dictionary<string, double>(){{GameResources.COINS, 40}},
                5 or 6 or 7 => new Dictionary<string, double>(){{GameResources.COINS, 50}},
                8 or 9 => new Dictionary<string, double>(){{GameResources.COINS, 120}},
                10 or 11 or 12 => new Dictionary<string, double>(){{GameResources.COINS, 200}, {GameResources.MEDALS, 3}},
                > 12 => new Dictionary<string, double>(){{GameResources.COINS, 500}, {GameResources.MEDALS, 7}},
                _ => new Dictionary<string, double>(){{GameResources.COINS, 999999999}}
            };
        }

        public void Serialize(FastBufferWriter writer)
        {
            var serializedLoadout = SkillTypeMapper.ToIdArray(loadout.ToArray());
            var serializedOwnedSkills = SkillTypeMapper.ToIdArray(ownedSkills.ToArray());
            
            writer.WriteValueSafe(serializedLoadout);
            writer.WriteValueSafe(serializedOwnedSkills);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.ReadValueSafe(out uint[] serializedLoadout);
            reader.ReadValueSafe(out uint[] serializedOwnedSkills);
            
            loadout = SkillTypeMapper.ToTypeArray(serializedLoadout).ToList();
            ownedSkills = SkillTypeMapper.ToTypeArray(serializedOwnedSkills).ToHashSet();
        }
    }
}