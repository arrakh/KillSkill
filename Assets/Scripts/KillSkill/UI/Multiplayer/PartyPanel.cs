using System;
using System.Collections.Generic;
using KillSkill.Network;
using KillSkill.SessionData.Implementations;
using UnityEngine;

namespace KillSkill.UI.Multiplayer
{
    public class PartyPanel : MonoBehaviour
    {
        [SerializeField] private PartyPanelElement prefab;
        [SerializeField] private RectTransform parent;
        [SerializeField] private PartyPanelElement localElement;

        private Dictionary<ulong, PartyPanelElement> spawnedElement = new(); 

        public void Display(NetworkPartySessionData partySession, NetworkIdSessionData idSessionData)
        {
            ClearElements();

            var localId = idSessionData.ClientId;
            Debug.Log($"[PP] Local ID is {localId}");
            
            foreach (var user in partySession.Party)
            {
                if (user.NetworkId.ClientId.Equals(localId))
                {
                    localElement.Display(true, user);
                    continue;
                }
                
                var element = Instantiate(prefab, parent, false);
                element.Display(false, user);
                spawnedElement[user.NetworkId.ClientId] = element;
            }
        }

        private void ClearElements()
        {
            foreach (var element in spawnedElement.Values)
                Destroy(element.gameObject);
            
            spawnedElement.Clear();
        }
    }
}