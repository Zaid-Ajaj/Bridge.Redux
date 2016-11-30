using Bridge;
using System;
using Bridge.Html5;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redux
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

namespace Redux
{
    [ObjectLiteral]
    public class ReduxLoggerOuput<T>
    {
        public T StateBefore { get; set; }
        public T StateAfter { get; set; }
        public object ActionDispatched { get; set; }
    }
    public class ReduxMiddleware
    {
        public static ReduxMiddleware From<TState>(Action<Store<TState>, dynamic, dynamic> func)
        {
            Func<Store<TState>, Func<dynamic, Action<dynamic>>> middleware =
                store =>
                {
                    return next =>
                    {
                        return action =>
                        {
                            func(store, next, action);
                        };
                    };
                };

            return Script.Write<ReduxMiddleware>("middleware");
        }


        public static readonly ReduxMiddleware Thunk = From<dynamic>((store, next, action) =>
        {
            /*@
            if (typeof action === 'function') { 
                return action(store.dispatch, store.getState);
            }

            next(action);

             */
        });

        /// <summary>
        /// Logs current state, then the action being dispatched, then the state after reduction
        /// </summary>
        public static readonly ReduxMiddleware DefaultLogger = From<dynamic>((store, next, action) =>
        {
            /*@
                console.log(store.getState());
                console.log(action);
                next(action)
                console.log(store.getState());
             */
        });

        public static ReduxMiddleware Logger<T>(Action<ReduxLoggerOuput<T>> logHandler)
        {
            /*@
             return Redux.ReduxMiddleware.from(Object, function (store, dipatchNext, action) {
                var stateBefore = store.getState();
                dipatchNext(action);
                var stateAfter = store.getState();

                var loggerOutput = { 
                   stateBefore: stateBefore,
                   actionDispatched: action,
                   stateAfter: stateAfter
                };

                logHandler(loggerOutput);

             });
            */

            return Script.Write<ReduxMiddleware>("");
        }
    }
}



namespace Redux
{
    /// <summary>
    /// DSL to create redux reducers using pattern matching on the types of actions 
    /// </summary>
    /// <typeparam name="TState">The type of the state tree object</typeparam>
    public class ReducerBuilder<TState>
    {
        private Dictionary<string, object> reducersDict;
        private string undefinedStateTypeName = "Redux.StateUndefined";
        private string unknownActionTypeName = "Redux.ActionUndefined";

        public ReducerBuilder()
        {
            reducersDict = new Dictionary<string, object>();
        }

        public ReducerBuilder<TState> WhenActionHasType<TAction>(Func<TState, TAction, TState> reducer)
        {
            reducersDict.Add(typeof(TAction).FullName, reducer);
            return this;
        }

        public ReducerBuilder<TState> WhenStateIsUndefinedOrNull(Func<TState> reducer)
        {
            Func<TState, object, TState> actualReducer = (state, action) => reducer();
            if (!reducersDict.ContainsKey(undefinedStateTypeName))
            {
                // it wasn't defined earlier, so add the reducer the dictionary!
                reducersDict.Add(undefinedStateTypeName, actualReducer);
            }
            else
            {
                // it has already been added, then overwrite it
                reducersDict[undefinedStateTypeName] = actualReducer;
            }
            return this;
        }


        bool IsUndefined<T>(T value)
        {
            return Script.Write<bool>("value === undefined");
        }

        public ReducerBuilder<TState> WhenActionHasType<TAction>(Func<TState, TState> reducer)
        {
            Func<TState, TAction, TState> actualReducer = (state, action) => reducer(state);

            reducersDict.Add(typeof(TAction).FullName, actualReducer);
            return this;
        }


        public ReducerBuilder<TState> WhenActionIsUnknown(Func<TState, TState> reducer)
        {
            // forget about the second argument
            Func<TState, object, TState> actualReducer = (state, action) => reducer(state);
            if (!reducersDict.ContainsKey(unknownActionTypeName))
            {
                // it wasn't defined earlier, so add the reducer the dictionary!
                reducersDict.Add(unknownActionTypeName, actualReducer);
            }
            else
            {
                // it has already been added, then overwrite it
                reducersDict[unknownActionTypeName] = actualReducer;
            }
                
            return this;
        }

        public ReduxReducer<TState> Build()
        {
            Func<TState, object, TState> pureReducer = (state, action) =>
            {
                if (reducersDict.ContainsKey(undefinedStateTypeName) && (IsUndefined(state) || state == null))
                {
                    var func = (Func<TState, object, TState>)reducersDict[undefinedStateTypeName];
                    return func(state, action);
                }

                var typeName = Script.Write<string>("action.type");
                if (reducersDict.ContainsKey(typeName))
                {
                    // then the type was known!
                    var func = (Func<TState, object, TState>)reducersDict[typeName];
                    return func(state, action);
                }
                else if (!reducersDict.ContainsKey(typeName) && reducersDict.ContainsKey(unknownActionTypeName))
                {
                    var func = (Func<TState, object, TState>)reducersDict[unknownActionTypeName];
                    return func(state, action);
                }
                else
                {
                    return state;
                }
            };

            return Script.Write<ReduxReducer<TState>>("pureReducer");
        }
    }
} 

namespace Redux
{
    [Name("Redux")]
    [External]
    public static class Redux
    {
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer);
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer, TState initialState);
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer, StoreMiddleware middleware);
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer, TState initialState, StoreMiddleware middleware);
        [Name("applyMiddleware")]
        public static extern StoreMiddleware ApplyMiddleware(params ReduxMiddleware[] middlewares);
        [Name("combineReducers")]
        public static extern ReduxReducer<TState> CombineReducers<TState>(TState state);
    }
}

namespace Redux
{
    [External]
    public sealed class StoreMiddleware { }
}

namespace Redux
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

namespace Redux
{
    [External]
    public class Store<TState>
    {
        [Name("getState")]
        public extern TState GetState();

        [Name("subscribe")]
        public extern void Subscribe(Action action);

        [Name("dispatch")]
        public extern void Dispatch<T>(Action<Action<T>> dispatch);

        [Name("dispatch")]
        public extern void Dispatch<T>(Action<Action<T>, Func<TState>> dipatch);
    }
}