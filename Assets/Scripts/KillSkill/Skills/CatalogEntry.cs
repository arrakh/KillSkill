using System.Collections.Generic;

namespace Skills
{
    public struct CatalogEntry
    {
        private bool purchasable;

        public CatalogEntry(string archetypeId, IReadOnlyDictionary<string, double> resourceCosts)
        {
            this.resourceCosts = resourceCosts;
            this.archetypeId = archetypeId;
            purchasable = true;
        }

        public bool Purchasable => purchasable;
        
        public IReadOnlyDictionary<string, double> resourceCosts;
        public string archetypeId;

        public static CatalogEntry NonPurchasable => new() { purchasable = false };
        public static CatalogEntry UnlockedFromStart(string archetypeId) => new(archetypeId, new Dictionary<string, double>());
    }
}