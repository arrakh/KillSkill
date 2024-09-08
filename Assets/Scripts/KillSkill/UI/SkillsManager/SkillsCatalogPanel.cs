using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KillSkill.Database;
using KillSkill.SessionData.Implementations;
using KillSkill.Skills;
using KillSkill.Utility;
using Skills;
using UnityEngine;

namespace KillSkill.UI.SkillsManager
{
    public class SkillsCatalogPanel : MonoBehaviour
    {
        [SerializeField] private GameObject catalogElementPrefab;
        [SerializeField] private Transform catalogElementParent;
        
        private Dictionary<string /*archetypeId*/, SkillCatalogArchetypeElement> spawnedElements = new();

        public void Display(SkillsSessionData skillsSession)
        {
            CleanElements();
            
            var allSkills = ReflectionCache.GetAll<Skill>();

            var skillsToSpawn = new List<Skill>();
            
            foreach (var skillType in allSkills)
            {
                var instance = Activator.CreateInstance(skillType);
                if (instance is not Skill skill) throw new Exception($"Trying to display catalog but Type {skillType} is not a SKILL");
                if (skill.CatalogEntry.hideInCatalog) continue;

                skillsToSpawn.Add(skill);    
            }

            foreach (var skill in skillsToSpawn.OrderBy(x => x.CatalogEntry.order))
            {
                var archetype = skill.CatalogEntry.archetypeId;
                
                if (!ArchetypesDatabase.TryGet(archetype, out var archetypeData))
                    Debug.LogError($"{skill.Metadata.name} does NOT belong to any archetype with id {skill.CatalogEntry.archetypeId}");
                
                //Debug.Log($"[SKILL] {skill.Metadata.name} - {archetypeData.name}");

                if (!spawnedElements.TryGetValue(archetype, out var element))
                    element = CreateNewArchetype(skillsSession, archetypeData);
                
                element.AddSkill(skill);
            }
        }

        private SkillCatalogArchetypeElement CreateNewArchetype(SkillsSessionData skillsSession, ArchetypeData archetypeData)
        {
            var obj = Instantiate(catalogElementPrefab, catalogElementParent);
            var element = obj.GetComponent<SkillCatalogArchetypeElement>();
            element.Display(skillsSession, archetypeData);
            spawnedElements[archetypeData.id] = element;
            return element;
        }

        private void CleanElements()
        {
            foreach (var element in spawnedElements.Values)
                Destroy(element.gameObject);
            
            spawnedElements.Clear();
        }

        public void Highlight(Skill skill)
        {
            foreach (var (_, element) in spawnedElements)
                element.SetHighlights(skill);
        }
    }
}