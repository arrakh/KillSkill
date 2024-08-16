using System;
using System.Collections.Generic;
using System.Linq;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Characters;
using KillSkill.Modules.Loaders;
using KillSkill.Modules.Loaders.Events;
using KillSkill.Network;
using KillSkill.Network.Messages;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Navigation;
using KillSkill.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillSkill.UI.Arena
{
    public class ArenaView : View
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private RectTransform elementParent;

        private List<ArenaCatalogElement> spawnedElements = new();

        private BattleSessionData battleSession;
        
        public void Display(BattleSessionData session, MilestonesSessionData milestones)
        {
            battleSession = session;
            var allMonsters = ReflectionCache.GetAll<INpcDefinition>();

            CleanObjects();

            var list = new List<(ICataloguedEnemy, INpcDefinition)>();
            
            foreach (var monster in allMonsters)
            {
                if (!typeof(ICataloguedEnemy).IsAssignableFrom(monster)) continue;
                var instance = Activator.CreateInstance(monster);
                if (instance is INpcDefinition enemyData && instance is ICataloguedEnemy catalogued) 
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
            if (!Net.IsServer()) return;
            
            battleSession.SetBattle(element.Data);

            Net.Server.Broadcast(new SwitchContextMessage(ContextType.Battle));
        }

        private void CleanObjects()
        {
            foreach (var element in spawnedElements)
                Destroy(element.gameObject);
            
            spawnedElements.Clear();
        }
    }
}