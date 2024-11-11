using System;
using System.Collections.Generic;

namespace Skills
{
    public struct CatalogEntry
    {
        public bool hideInCatalog;
        public int order;
        public string archetypeId;

        public IReadOnlyDictionary<string, double> resourceCosts;
        public string[] requiredMilestones;

        public CatalogEntry(string archetypeId, int order, IReadOnlyDictionary<string, double> resourceCosts = null, string[] requiredMilestones = null)
        {
            hideInCatalog = false;
            this.order = order;
            this.archetypeId = archetypeId;
            this.resourceCosts = resourceCosts ?? new Dictionary<string, double>();
            this.requiredMilestones = requiredMilestones ?? Array.Empty<string>();
        }

        public static CatalogEntry NonPurchasable => new() { hideInCatalog = true, requiredMilestones = Array.Empty<string>()};
        public static CatalogEntry UnlockedFromStart(string archetypeId, int order = int.MinValue) => new ()
        {
            archetypeId = archetypeId, hideInCatalog = false,
            resourceCosts = new Dictionary<string, double>(),
            requiredMilestones = Array.Empty<string>(),
            order = order
        };
    }
}