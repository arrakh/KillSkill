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
        
        public void Display(BattleSessionData session, MilestonesSessionData milestones)
        {
            battleSession = session;
            var allMonsters = ReflectionUtility.GetAll<IEnemyData>();

            CleanObjects();

            var list = new List<(ICataloguedEnemy, IEnemyData)>();
            
            foreach (var monster in allMonsters)
            {
                if (!typeof(ICataloguedEnemy).IsAssignableFrom(monster)) continue;
                var instance = Activator.CreateInstance(monster);
                if (instance is IEnemyData enemyData && instance is ICataloguedEnemy catalogued) 
                    list.Add((catalogued, enemyData));
            }

            foreach (var enemyPair in list.OrderBy(x => x.Item1.CatalogOrder))
            {
                var (catalogue, enemy) = enemyPair;
                if (!milestones.HasAll(catalogue.RequiredMilestones?.ToArray())) continue;
                
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