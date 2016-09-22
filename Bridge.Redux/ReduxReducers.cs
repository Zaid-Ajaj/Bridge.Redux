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

    public static class ReduxReducers
    {
        [IgnoreGeneric]
        public static ReduxReducer<TAppState> Create<TAppState, TActionType>(Func<TAppState, TActionType, TAppState> reducer)
        {
            return Script.Write<ReduxReducer<TAppState>>("reducer");
        }
    }
}