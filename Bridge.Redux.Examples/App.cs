using Bridge.Html5;
using System;

namespace Bridge.Redux.Examples
{
    public class App
    {
        public static void Main()
        {
            var appReducer = Redux.CombineReducers(new AppState
            {
                User = Reducers.UserReducer(),
                Counter = Reducers.CounterReducer()
            });

            var middleware = Redux.ApplyMiddleware(Middleware.Thunk);

            var store = Redux.CreateStore(appReducer, middleware);

            store.Subscribe(() => Console.WriteLine(store.GetState()));

            store.Dispatch(Actions.Increment());
            store.Dispatch(Actions.Increment());
            store.Dispatch(Actions.Decrement());
            store.Dispatch(Actions.SetUserName("Zaid"));
            store.Dispatch(Actions.SetUserAge(19));

            store.Dispatch<ActionTypes>(dispatch =>
            {
                Console.WriteLine("Beginning 'async' operation");
                Window.SetTimeout(() =>
                {
                    Console.WriteLine("Dispatching 'Increment'");
                    dispatch(Actions.Increment());
                }, 5000);
            });

            store.Dispatch<ActionTypes>((dispatch, getState) =>
            {
                Console.WriteLine("Beginning 'async' operation");
                Window.SetTimeout(() =>
                {
                    Console.WriteLine(getState());
                    Console.WriteLine("Dispatching 'Increment'");
                    dispatch(Actions.Increment());
                }, 5000);
            });
        }

    }
}