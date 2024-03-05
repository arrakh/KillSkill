using System;
using System.Collections.Generic;
using Arr.EventsSystem;
using DefaultNamespace.SessionData;
using KillSkill.Constants;
using KillSkill.SessionData;
using KillSkill.SessionData.Events;
using UnityEngine;

namespace SessionData.Implementations
{
    public class ResourcesSessionData : ISessionData
    {
        private Dictionary<string, double> resources = new()
        {
            {GameResources.COINS, 5}
        };

        public IReadOnlyDictionary<string, double> Resources => resources;

        public void AddResource(string id, double amount)
        {
            var finalAmount = amount;
            if (resources.TryGetValue(id, out var existing)) finalAmount = existing + amount;
            resources[id] = finalAmount;
            GlobalEvents.Fire(new ResourcesUpdatedEvent(this));
        }

        public void RemoveResources(IReadOnlyDictionary<string, double> toRemove)
        {
            foreach (var (id, amount) in toRemove)
            {
                if (!resources.TryGetValue(id, out var existing))
                    throw new Exception("Trying to Remove Resources but resource does not exist! Make sure you check CanAfford before calling");

                var finalAmount = Math.Max(existing - amount, 0);
                resources[id] = finalAmount;
            }
            
            GlobalEvents.Fire(new ResourcesUpdatedEvent(this));
        }

        public bool CanAfford(IReadOnlyDictionary<string, double> cost)
        {
            foreach (var (id, amount) in cost)
            {
                if (!resources.TryGetValue(id, out var existingAmount)) return false;
                if (existingAmount < amount) return false;
            }

            return true;
        }

        public void OnLoad()
        {
            
        }

        public void OnUnload()
        {
            
        }
    }
}