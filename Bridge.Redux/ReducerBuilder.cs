using System;
using System.Collections.Generic;



namespace Bridge.Redux
{
    /// <summary>
    /// DSL to create redux reducers using pattern matching on the types of actions 
    /// </summary>
    /// <typeparam name="TState">The type of the state tree object</typeparam>
    public class ReducerBuilder<TState>
    {
        private Dictionary<string, object> reducersDict;
        private string undefinedStateTypeName = "Bridge.Redux.StateUndefined";
        private string unknownActionTypeName = "Bridge.Redux.ActionUndefined";

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
                if (reducersDict.ContainsKey(undefinedStateTypeName) && (state.IsUndefined() || state == null))
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