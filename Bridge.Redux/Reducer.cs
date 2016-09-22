using System;

namespace Bridge.Redux
{
    [IgnoreGeneric]
    public sealed class ReduxReducer<T>
    {
        public ReduxReducer() { }

        [IgnoreGeneric]
        public static implicit operator T(ReduxReducer<T> reducer)
        {
            return Script.Write<T>("reducer");
        }

        //[IgnoreGeneric]
        //public static Reducer<T> Create(Func<T, dynamic, T> reducer)
        //{
        //    return Script.Write<Reducer<T>>("reducer");
        //}

        //[IgnoreGeneric]
        //public static Reducer<T> Create<TAction>(Func<T, TAction, T> reducer)
        //{
        //    return Script.Write<Reducer<T>>("reducer");
        //}
    }

    public class ReduxReducers
    {
        [IgnoreGeneric]
        public static ReduxReducer<TAppState> Create<TAppState>(Func<TAppState, dynamic, TAppState> reducer)
        {
            return Script.Write<ReduxReducer<TAppState>>("reducer");
        }

        [IgnoreGeneric]
        public static ReduxReducer<TAppState> Create<TAppState, TActionType>(Func<TAppState, TActionType, TAppState> reducer)
        {
            return Script.Write<ReduxReducer<TAppState>>("reducer");
        }
    }
}