namespace Arr.EventsSystem
{
    public static class GlobalEvents
    {
        private static EventHandler _handler = new EventHandler();

        public static EventHandler Instance => _handler;

        public static void Fire<TParam>(TParam data) where TParam : struct => _handler.Fire(data);
        public static TReturn Query<TReturn>() => _handler.Query<TReturn>();
        public static TReturn Query<TReturn, TParam>(TParam data) where TParam : struct => _handler.Query<TReturn, TParam>(data);
        
        public static void Register<TParam>(IEventListener<TParam> listener) where TParam : struct => _handler.Register(listener);
        public static void RegisterQuery<TReturn>(IQueryProvider<TReturn> provider) => _handler.Register(provider);
        public static void RegisterQuery<TReturn, TParam>(IQueryProvider<TReturn, TParam> provider) where TParam : struct => _handler.Register(provider);
        
        public static void Unregister<TParam>(IEventListener<TParam> listener) where TParam : struct => _handler.Unregister(listener);
        public static void UnregisterQuery<TReturn>() => _handler.Unregister<TReturn>();
        public static void UnregisterQuery<TReturn, TParam>() where TParam : struct => _handler.Unregister<TReturn, TParam>();
    }
}