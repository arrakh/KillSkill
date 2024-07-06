using KillSkill.Characters;
using KillSkill.Characters.Implementations.EnemyData;
using StatusEffects;
using UnityEngine;

namespace KillSkill.Skills.Implementations.Enemy
{
    public class SummonFlyingEyeSkill : Skill
    {
        private const float CAST_DURATION = 2f;

        protected override float CooldownTime => 10f;

        private ICharacterFactory factory;
        private ICharacter casterChar;
        private ICharacter targetChar;

        public override void Execute(ICharacter caster, ICharacter target)
        {
            factory = caster.CharacterFactory;
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(CAST_DURATION, OnCast));
        }

        private void OnCast()
        {
            var character = factory.CreateNpc(new FlyingEye());
            character.Position = GetRandomPosition(casterChar.Position);
            character.SetTarget(targetChar);
        }

        private Vector3 GetRandomPosition(Vector3 origin)
        {
            var randX = Random.Range(-2f, 2f);
            var randY = 3f + Random.Range(-1f, 1f);
            return origin + new Vector3(randX, randY, 0);
        }
    }
}