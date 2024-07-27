using System;
using System.Collections.Generic;
using System.Linq;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Modules.Loaders.Events;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Navigation
{
    public class NavigationView : View
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private RectTransform parent;
        [SerializeField] private Button quitButton;
        
        private Dictionary<string, INavigateSection> sections = new();
        private Dictionary<string, NavigationElement> spawnedElements = new();

        private INavigateSection lastSelected = null;

        private void Awake()
        {
            quitButton.onClick.AddListener(OnQuit);
        }

        private void OnQuit()
        {
            GlobalEvents.Fire(new QuitLobbyEvent());
        }

        public void AddSection(INavigateSection section)
        {
            CleanObjects();

            sections[section.Name] = section;

            foreach (var toSpawn in sections.Values.OrderBy(x => x.Order))
            {
                var obj = Instantiate(elementPrefab, parent);
                var element = obj.GetComponent<NavigationElement>();
                element.Display(toSpawn, OnNavigationClicked);
                spawnedElements[toSpawn.Name] = element;

                if (lastSelected == null) continue;
                
                element.SetHighlight(toSpawn.Name == lastSelected.Name);
            }
            
            Invoke(nameof(DelayedRebuild), 0f);
        }

        void DelayedRebuild()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
        
        private void OnNavigationClicked(NavigationElement element) => Select(element.Section);

        public void Select(INavigateSection toSelect)
        {
            if (lastSelected != null && lastSelected.Name == toSelect.Name) return;
            
            lastSelected = toSelect;
            
            foreach (var section in sections)
            {
                var selected = section.Key == toSelect.Name;
                spawnedElements[section.Key].SetHighlight(selected);
                section.Value.OnNavigate(selected);
            }
        }

        private void CleanObjects()
        {
            foreach (var element in spawnedElements.Values)
                Destroy(element.gameObject);
            
            spawnedElements.Clear();
        }
    }
}