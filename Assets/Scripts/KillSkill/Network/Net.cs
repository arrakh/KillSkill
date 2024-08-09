using Arr.EventsSystem;
using KillSkill.Modules.Network.Events;

namespace KillSkill.Network
{
    public static class Net
    {
        public static bool IsServer() => GlobalEvents.Query<IsServerQuery>().isServer;
        
        public static bool IsClient() => GlobalEvents.Query<IsClientQuery>().isClient;
        
        public static class Client
        {
            public static void Send(INetCodeMessage message) => GlobalEvents.Fire(new SendMessageEvent(0, message));
        }   
        
        public static class Server
        {
            public static void Send(ulong clientId, INetCodeMessage message) => GlobalEvents.Fire(new SendMessageEvent(clientId, message));

            public static void Broadcast(INetCodeMessage message) => GlobalEvents.Fire(new BroadcastMessageEvent(message));
        }   
    }
}