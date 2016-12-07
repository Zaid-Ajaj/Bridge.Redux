using Bridge.Html5;
using System;

namespace Bridge.Redux
{
    public static class Extensions
    {
        public static void Dispatch<T, U>(this Store<T> store, U action)
        {

            if (action == null) throw new ArgumentNullException(nameof(action));
            if (typeof(U).FullName == "Object") throw new Exception("Action type cannot be 'Object' when dispatching a typed action where the full name of the action is needed, Consider using 'DispatchPlainObject'");


            Script.Write("action = JSON.parse(JSON.stringify(action))");
            action["type"] = typeof(U).FullName;
            Script.Write("store.dispatch(action)");
        }




        public static TState Apply<TState, TAction>(this ReduxReducer<TState> reducer, TState state, TAction action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
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