using System;
using System.Collections.Generic;



namespace Bridge.Redux
{

    public class ReducerBuilder<TState>
    {
        private Dictionary<string, object> reducersDict;

        public ReducerBuilder()
        {
            reducersDict = new Dictionary<string, object>();
        }

        public ReducerBuilder<TState> WhenActionHasType<TAction>(Func<TState, TAction, TState> reducer)
        {
            reducersDict.Add(typeof(TAction).FullName, reducer);
            foreach(var value in reducersDict)
            {
                Console.WriteLine($"{value.Key} => {value.Value}");
            }
            return this;
        }

        public ReducerBuilder<TState> WhenActionHasType<TAction>(Func<TState, TState> reducer)
        {
            Func<TState, TAction, TState> actualReducer = (state, action) => reducer(state);

            reducersDict.Add(typeof(TAction).FullName, actualReducer);
            return this;
        }


        //public ReducerBuilder<TState> WhenActionIsUnknown(Func<TState, TState> reducer)
        //{
        //    // forget about the second argument
        //    Func<TState, object, TState> actualReducer = (state, action) => reducer(state);
        //    reducersDict.Add("", actualReducer);
        //    return this;
        //}

        public ReduxReducer<TState> Build()
        {
            Func<TState, object, TState> pureReducer = (state, action) =>
            {
                var typeName = Script.Write<string>("action.type");
                if (reducersDict.ContainsKey(typeName))
                {
                    var func = (Func<TState, object, TState>)reducersDict[typeName];
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