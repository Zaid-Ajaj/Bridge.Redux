namespace Bridge.Redux.Examples.Basic
{
    public static partial class Reducers
    {
        public static ReduxReducer<User> UserReducer()
        {
            return ReduxReducers.Create<User, SimpleAction>((state, action) =>
            {
                if (state.IsUndefined())
                    return new User { };

                if (action.Type == ActionTypes.SetName)
                    return new User { Name = action.Name, Age = state.Age };

                if (action.Type == ActionTypes.SetAge)
                    return new User { Name = state.Name, Age = action.Age };

                return state;
            });
        }
    }
}