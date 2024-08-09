using UnityEngine;

namespace KillSkill.Modules.Network.Events
{
    public class IsServerQuery
    {
        public bool isServer;

        public IsServerQuery(bool isServer)
        {
            this.isServer = isServer;
        }
    }
}