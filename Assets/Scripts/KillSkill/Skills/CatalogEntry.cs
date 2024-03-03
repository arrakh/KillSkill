using System.Collections.Generic;

namespace Skills
{
    public struct CatalogEntry
    {
        public bool hideInCatalog;
        public int order;
        
        public IReadOnlyDictionary<string, double> resourceCosts;
        public string archetypeId;

        public static CatalogEntry NonPurchasable => new() { hideInCatalog = true };
        public static CatalogEntry UnlockedFromStart(string archetypeId) => new ()
        {
            archetypeId = archetypeId, hideInCatalog = false,
            resourceCosts = new Dictionary<string, double>(),
            order = int.MinValue
        };
    }
}