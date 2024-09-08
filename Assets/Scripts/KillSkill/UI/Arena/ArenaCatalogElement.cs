using System;
using KillSkill.Characters;
using KillSkill.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Arena
{
    public class ArenaCatalogElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI enemyText;
        [SerializeField] private Image enemyIcon;
        [SerializeField] private Button button;


        public INpcDefinition Data => data;
        
        private INpcDefinition data;
        private Action<ArenaCatalogElement> onButtonClick;

        public void Display(INpcDefinition inpcDefinition, Action<ArenaCatalogElement> onClick)
        {
            data = inpcDefinition;
            onButtonClick = onClick;

            var flipbook = CharacterFlipBooksDatabase.Get(inpcDefinition.Id);
            enemyIcon.sprite = flipbook.Default.GetFrame(0);
            enemyText.text = data.DisplayName;
        }
        
        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            onButtonClick?.Invoke(this);
        }
    }
}