namespace Arr.EventsSystem
{
    public interface IEventListener<in TParam> where TParam : struct
    {
        public void OnEvent(TParam data);
    }

    public interface IQueryProvider<out TReturn>
    {
        public TReturn OnQuery();
    }

    public interface IQueryProvider<out TReturn, in TParam> where TParam : struct
    {
        public TReturn OnQuery(TParam data);
    }
}