using System;

namespace Arr.SDS
{
    public interface IScriptableKey
    {
        public string Id { get; }
    }

    public interface IScriptableKey<out T> where T : IEquatable<T>
    {
        public T Key { get; }
    }
}