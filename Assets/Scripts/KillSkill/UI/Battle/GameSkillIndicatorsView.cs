using System.Collections.Generic;
using Arr.ViewModuleSystem;
using KillSkill.Characters;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Game;
using UnityEngine;

namespace KillSkill.UI.Battle
{
    public class GameSkillIndicatorsView : View
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private RectTransform parent;
        [SerializeField] private PlayerCharacter character;
        
        private List<GameSkillIndicator> spawnedIndicators = new();

        public void AnimateSkill(int index)
        {
            if (index >= spawnedIndicators.Count) return;
            spawnedIndicators[index].AnimateTap();
        }

        public void Display(ICharacter t)
        {
            var skillSession = Session.GetData<SkillsSessionData>();
            for (int i = 0; i < skillSession.SlotCount; i++)
            {
                var obj = Instantiate(prefab, parent);
                var indicator = obj.GetComponent<GameSkillIndicator>();
                spawnedIndicators.Add(indicator);
                indicator.Display(i, t);
            }
        }
    }
}