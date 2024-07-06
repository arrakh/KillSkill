using System;
using System.Collections.Generic;

namespace Arr
{
    [Serializable]
    public class WeightTable<T>
    {
        private IReadOnlyDictionary<T, int> table;
        private T defaultItem;
        private int totalValue;

        public T DefaultItem => defaultItem;

        public IReadOnlyDictionary<T, int> Table => table;

        public WeightTable(T defaultItem, IReadOnlyDictionary<T, int> table)
        {
            this.table = table;
            this.defaultItem = defaultItem;

            foreach (var entry in table)
                totalValue += entry.Value;
        }

        public T Evaluate(int value)
        {
            var current = 0;
            foreach (var entry in table)
            {
                current += entry.Value;
                if (value < current) return entry.Key;
            }

            return defaultItem;
        }

        public T GetRandom()
        {
            var rnd = new Random();
            return Evaluate(rnd.Next(totalValue));
        }

        public int GetTotalWeight() => totalValue;
    }
}