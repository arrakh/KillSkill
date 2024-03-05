using System.Collections.Generic;
using Arr.ViewModuleSystem;
using KillSkill.SessionData.Implementations;
using KillSkill.Skills;
using KillSkill.UI.Navigation;
using SessionData.Implementations;
using Skills;
using UnityEngine;

namespace KillSkill.UI.SkillsManager
{
    public class SkillsManagerView : View, INavigateSection
    {
        [SerializeField] private SkillsCatalogPanel catalogPanel;
        [SerializeField] private ResourcesPanel resourcesPanel;
        [SerializeField] private SkillPreviewPanel skillPreviewPanel;
        [SerializeField] private SkillsLoadoutPanel loadoutPanel;
        
        public void Display(ResourcesSessionData resources, SkillsSessionData skills)
        {
            catalogPanel.Display(skills);
            resourcesPanel.Display(resources.Resources);
            loadoutPanel.Display(skills);
            skillPreviewPanel.UpdateDisplay(skills);
        }

        public void PreviewSkill(Skill skill, SkillsSessionData skillSession)
        {
            skillPreviewPanel.Display(skill, skillSession);
            catalogPanel.Highlight(skill);
        }

        //todo: Should sit in the view module
        public Skill GetCurrentlyDisplayed() => skillPreviewPanel.CurrentSkill;
        
        //todo: Should sit in the view module
        int INavigateSection.Order => 1;
        string INavigateSection.Name => "Skills";
        void INavigateSection.OnNavigate(bool selected)
        {
            gameObject.SetActive(selected);
        }
    }
}