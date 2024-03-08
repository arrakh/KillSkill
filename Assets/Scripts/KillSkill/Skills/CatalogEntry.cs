using System.Collections.Generic;

namespace Skills
{
    public struct CatalogEntry
    {
        public bool hideInCatalog;
        public int order;
        public string archetypeId;

        public IReadOnlyDictionary<string, double> resourceCosts;

        public static CatalogEntry NonPurchasable => new() { hideInCatalog = true };
        public static CatalogEntry UnlockedFromStart(string archetypeId, int order = int.MinValue) => new ()
        {
            archetypeId = archetypeId, hideInCatalog = false,
            resourceCosts = new Dictionary<string, double>(),
            order = order
        };
    }
}