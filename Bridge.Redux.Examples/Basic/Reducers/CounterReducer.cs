namespace Bridge.Redux.Examples.Basic
{
    public static partial class Reducers
    {
        public static ReduxReducer<Counter> CounterReducer()
        {
            return ReduxReducers.Create<Counter, SimpleAction>((state, action) =>
            {
                if (state.IsUndefined())
                    return new Counter { Value = 0 };

                if (action.Type == ActionTypes.Increment)
                    return new Counter { Value = state.Value + 1 };

                if (action.Type == ActionTypes.Decrement)
                    return new Counter { Value = state.Value - 1 };

                return state;
            });
        }
    }
} 