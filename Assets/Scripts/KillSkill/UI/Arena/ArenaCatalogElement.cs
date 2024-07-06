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


        public IEnemyData Data => data;
        
        private IEnemyData data;
        private Action<ArenaCatalogElement> onButtonClick;

        public void Display(IEnemyData enemyData, Action<ArenaCatalogElement> onClick)
        {
            data = enemyData;
            onButtonClick = onClick;

            var flipbook = CharacterFlipBooksDatabase.Get(enemyData.Id);
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