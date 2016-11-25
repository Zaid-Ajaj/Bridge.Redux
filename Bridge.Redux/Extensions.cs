using Bridge.Html5;
using System;

namespace Bridge.Redux
{
    public static class Extensions
    {
        public static void Dispatch<T, U>(this Store<T> store, U action)
        {
            Script.Write("action = JSON.parse(JSON.stringify(action))");
            action["type"] = typeof(U).FullName;
            Script.Write("store.dispatch(action)");
        }

        public static TState Apply<TState, TAction>(this ReduxReducer<TState> reducer, TState state, TAction action)
        {
            Script.Write("action = JSON.parse(JSON.stringify(action))");
            action["type"] = typeof(TAction).FullName;
            return Script.Write<TState>("reducer(state, action)");
        }

        public static TAction NormalizeActionForDispatch<TAction>(this TAction action)
        {
            Script.Write("action = JSON.parse(JSON.stringify(action))");
            action["type"] = typeof(TAction).FullName;
            return action;
        } 

        public static void DispatchPlainObject<T, U>(this Store<T> store, U action)
        {
            Script.Write("store.dispatch(action)");
        }
    }
}