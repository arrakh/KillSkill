using System;
using System.Collections.Generic;
using System.Linq;
using Arr.ViewModuleSystem;
using KillSkill.Characters;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Navigation;
using KillSkill.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillSkill.UI.Arena
{
    public class ArenaView : View, INavigateSection
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private RectTransform elementParent;

        private List<ArenaCatalogElement> spawnedElements = new();

        private BattleSessionData battleSession;
        
        public void Display(BattleSessionData session)
        {
            battleSession = session;
            var allMonsters = ReflectionUtility.GetAll<IEnemyData>();

            CleanObjects();

            var list = new List<IEnemyData>();
            
            foreach (var monster in allMonsters)
            {
                var instance = Activator.CreateInstance(monster);
                if (instance is IEnemyData enemyData) list.Add(enemyData);
            }

            foreach (var enemy in list.OrderBy(x => x.CatalogOrder))
            {
                var obj = Instantiate(elementPrefab, elementParent);
                var element = obj.GetComponent<ArenaCatalogElement>();
                
                element.Display(enemy, OnElementClicked);
                spawnedElements.Add(element);
            }
        }

        private void OnElementClicked(ArenaCatalogElement element)
        {
            battleSession.SetBattle(element.Data);

            SceneManager.LoadScene("Game");
        }

        private void CleanObjects()
        {
            foreach (var element in spawnedElements)
                Destroy(element.gameObject);
            
            spawnedElements.Clear();
        }

        //todo: Should sit in the view module
        int INavigateSection.Order => 0;
        string INavigateSection.Name => "Arena";
        void INavigateSection.OnNavigate(bool selected)
        {
            gameObject.SetActive(selected);
        }
    }
}