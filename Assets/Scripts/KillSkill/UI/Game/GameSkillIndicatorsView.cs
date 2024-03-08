using System.Collections.Generic;
using Arr.ViewModuleSystem;
using KillSkill.Characters;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using UnityEngine;

namespace KillSkill.UI.Game
{
    //todo: MAKE MODULE
    public class GameSkillIndicatorsView : View
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private RectTransform parent;
        [SerializeField] private PlayerCharacter character;
        
        private List<GameSkillIndicator> spawnedIndicators = new();

        private void Start()
        {
            character.onInitialize.Subscribe(OnInitialized);
            character.OnSkillIndexPressed.AddListener(OnSkillIndexPressed);
        }

        private void OnSkillIndexPressed(int index)
        {
            if (index >= spawnedIndicators.Count) return;
            spawnedIndicators[index].AnimateTap();
        }

        private void OnInitialized(Character t)
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