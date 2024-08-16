using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using Arr.Utils;
using KillSkill.Modules.Network.Events;
using KillSkill.Network;
using KillSkill.Network.Messages;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using UnityEngine;

namespace KillSkill.Modules.Network
{
    public class CommitModule : BaseModule,
        IEventListener<NetMessageEvent<CommitMessage>>
    {
        private const int TIMEOUT_MS = 5000;
        
        private TaskCompletionSource<ulong[]> committedClientsTsc = new();
        private HashSet<ulong> committedClients = new();

        private NetworkPartySessionData party;

        public TaskCompletionSource<ulong[]> CommittedClientsTsc => committedClientsTsc;

        protected override async Task OnLoad()
        {
            party = Session.GetData<NetworkPartySessionData>();

            if (Net.IsServer()) ProcessServer().CatchExceptions();
            if (Net.IsClient()) Net.Client.Send(new CommitMessage());
        }

        private async Task ProcessServer()
        {
            var timeoutTask = Task.Delay(TIMEOUT_MS);
            bool allCommitted = false;
        
            while (!allCommitted && !timeoutTask.IsCompleted)
            {
                allCommitted = HasAllCommitted();
                await Task.Yield();
            }
            
            committedClientsTsc.TrySetResult(committedClients.ToArray());
            
            if (allCommitted) Debug.Log("[CM] All clients have committed.");
            else Debug.LogWarning("[CM] Timeout reached before all clients committed.");
        }

        private bool HasAllCommitted()
        {
            foreach (var user in party.Party)
                if (!committedClients.Contains(user.NetworkId.ClientId))
                    return false;

            return true;
        }

        public void OnEvent(NetMessageEvent<CommitMessage> data)
        {
            Debug.Log($"[CM] CLIENT WITH ID {data.senderId} HAS SENT COMMIT");
            committedClients.Add(data.senderId);
        }
    }
}