using System;

namespace Bridge.Redux
{
    [IgnoreGeneric]
    public sealed class Reducer<T>
    {
        public Reducer() { }

        [IgnoreGeneric]
        public static implicit operator T(Reducer<T> reducer)
        {
            return Script.Write<T>("reducer");
        }

        [IgnoreGeneric]
        public static Reducer<T> Create(Func<T, dynamic, T> reducer)
        {
            return Script.Write<Reducer<T>>("reducer");
        }

        [IgnoreGeneric]
        public static Reducer<T> Create<TAction>(Func<T, TAction, T> reducer)
        {
            return Script.Write<Reducer<T>>("reducer");
        }
    }
}