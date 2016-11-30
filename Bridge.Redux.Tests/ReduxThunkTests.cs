namespace Bridge.Redux.Tests
{
    using Html5;
    using QUnit;
    public static class ReduxThunkTests
    {
        public static void Run()
        {
            var initialCounter = new Counter { Count = 5 };

            var counterReducer =
                BuildReducer.For<Counter>()
                            .WhenActionHasType<Increment>((state, act) => new Counter { Count = state.Count + 1 })
                            .WhenActionHasType<Decrement>((state, act) => new Counter { Count = state.Count - 1 })
                            .WhenActionHasType<IncrementBy>((state, act) => new Counter { Count = state.Count + act.Value })
                            .Build();

            var middleware = Redux.ApplyMiddleware(ReduxMiddleware.Thunk);

            var store = Redux.CreateStore(counterReducer, initialCounter, middleware);

            QUnit.Module("Redux Thunk Middleware");

            QUnit.Test("Dispatching action with thunk middleware works", assert =>
            {
                store.Dispatch(new Increment());
                assert.Equal(store.GetState().Count, 6);
            });

            QUnit.Test("Dispatching action after a timeout (async) works", assert =>
            {
                var done = assert.Async();
                store.Dispatch<IncrementBy>(dispatch =>
                {
                    Window.SetTimeout(() =>
                    {
                        var action = new IncrementBy { Value = 10 };
                        var normalized = action.NormalizeActionForDispatch();
                        dispatch(normalized);
                        assert.Equal(store.GetState().Count, 16);
                        done();

                    }, 500);
                });
            });
        }
    }
}