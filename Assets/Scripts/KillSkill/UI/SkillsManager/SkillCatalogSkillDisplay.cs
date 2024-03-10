using UI.Tooltips;
using UnityEngine;

namespace KillSkill.UI.SkillsManager
{
    public class SkillCatalogSkillDisplay : SkillDisplay, ITooltipElement
    {
        public bool HasData() => skill != null && !skill.Metadata.isEmpty;

        public TooltipData GetData()
        {
            var desc = skill.Metadata;
            return new TooltipData(desc.icon, desc.name, desc.description, desc.extraDescription);
        }

        public int UniqueId => gameObject.GetInstanceID();
    }
}