using Bridge.Html5;
using System;

namespace Bridge.Redux
{
    public static class Extensions
    {
        public static bool IsUndefined<T>(this T value)
        {
            return Script.Write<bool>("value === undefined");
        }

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

        public static void DispatchPlainObject<T, U>(this Store<T> store, U action)
        {
            Script.Write("store.dispatch(action)");
        }

        // Works only with object literals
        public static T MergeWith<T>(this T value, T otherValue)
        {
            var result = Script.Write<T>("{ }");
            /*@
            for (var key in value) { 
                if (typeof value[key] === 'function') {
                    continue;
                }
                
                result[key] = value[key];
            
            }
           for (var key in otherValue) { 
                if (typeof otherValue[key] === 'function') {
                    continue;
                }
                
                result[key] = otherValue[key];
            
            }
             */
            return result;
        }
    }
}