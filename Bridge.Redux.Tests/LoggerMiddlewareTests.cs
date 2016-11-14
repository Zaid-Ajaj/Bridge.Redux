namespace Bridge.Redux.Tests
{
    using QUnit;
    public static class LoggerMiddlewareTests
    {
        private static void DefaultLoggerTests()
        {
            var initialCounter = new Counter { Count = 5 };

            var counterReducer =
                BuildReducer.For<Counter>()
                            .WhenActionHasType<Increment>((state, act) => new Counter { Count = state.Count + 1 })
                            .WhenActionHasType<Decrement>((state, act) => new Counter { Count = state.Count - 1 })
                            .WhenActionHasType<IncrementBy>((state, act) => new Counter { Count = state.Count + act.Value })
                            .Build();

            var middleware = Redux.ApplyMiddleware(ReduxMiddleware.DefaultLogger);

            var store = Redux.CreateStore(counterReducer, initialCounter, middleware);

            QUnit.Test("Dispatch works as expected when using default logger middleware", assert =>
            {
                store.Dispatch(new Increment());
                assert.Equal(store.GetState().Count, 6);
            });
        }


        private static void NonDefaultLoggerTests()
        {
            var initialCounter = new Counter { Count = 5 };

            var counterReducer =
                    BuildReducer
                        .For<Counter>()
                        .WhenActionHasType<Increment>((state, act) => new Counter { Count = state.Count + 1 })
                        .WhenActionHasType<Decrement>((state, act) => new Counter { Count = state.Count - 1 })
                        .WhenActionHasType<IncrementBy>((state, act) => new Counter { Count = state.Count + act.Value })
                        .Build();

            var reduxLog = new ReduxLoggerOuput<Counter> { };

            var logger = ReduxMiddleware.Logger<Counter>(log => reduxLog = log);

            var middleware = Redux.ApplyMiddleware(logger);

            var store = Redux.CreateStore(counterReducer, initialCounter, middleware);

            QUnit.Test("Non default logger works", assert =>
            {
                var stateCountBefore = store.GetState().Count;
                var action = (new Increment()).NormalizeActionForDispatch();
                var stateCountAfter = stateCountBefore + 1;

                store.Dispatch(action);
                assert.Equal(stateCountBefore, reduxLog.StateBefore.Count);
                assert.Equal(reduxLog.ActionDispatched["type"], action["type"]);
                assert.Equal(stateCountAfter, reduxLog.StateAfter.Count);
            });
        }

        public static void Run()
        {
            QUnit.Module("Redux Logger Middleware");
            DefaultLoggerTests();
            NonDefaultLoggerTests();
        }

    }
}