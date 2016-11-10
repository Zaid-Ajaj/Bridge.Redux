using System;

namespace Bridge.Redux
{
    [IgnoreGeneric]
    public sealed class ReduxReducer<T>
    {
        private ReduxReducer() { }

        [IgnoreGeneric]
        public static implicit operator T(ReduxReducer<T> reducer)
        {
            return Script.Write<T>("reducer");
        }
    }

    public static class BuildReducer
    {
        public static ReducerBuilder<TState> For<TState>() => new ReducerBuilder<TState>();
    }
} 