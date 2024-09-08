using System;
using KillSkill.Skills;
using Unity.Collections;
using Unity.Netcode;

namespace KillSkill.Characters
{
    public class CharacterData : INetworkSerializable
    {
        private FixedString64Bytes fixedId;
        private float health;
        private Type[] skillTypes;

        private uint[] skillTypeIds;

        public CharacterData()
        {
        }

        public string Id => fixedId.Value;
        public float Health => health;
        public Type[] SkillTypes => skillTypes;


        public CharacterData(string id, float health, Type[] skillTypes)
        {
            fixedId = id;
            this.health = health;
            this.skillTypes = skillTypes;
            skillTypeIds = SkillTypeMapper.ToIdArray(skillTypes);
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref fixedId);
            serializer.SerializeValue(ref health);
            serializer.SerializeValue(ref skillTypeIds);

            if (serializer.IsReader)
            {
                skillTypes = SkillTypeMapper.ToTypeArray(skillTypeIds);
            }
        }
    }
}