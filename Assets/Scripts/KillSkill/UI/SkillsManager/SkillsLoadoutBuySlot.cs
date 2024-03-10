using System;
using Arr.EventsSystem;
using KillSkill.UI.SkillsManager.Events;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.SkillsManager
{
    public class SkillsLoadoutBuySlot : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GlobalEvents.Fire(new PreviewBuySlotEvent());
        }
    }
}