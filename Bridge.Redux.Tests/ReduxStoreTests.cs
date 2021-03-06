﻿namespace Bridge.Redux.Tests
{
    using QUnit;
    using System;

    public static class ReduxStoreTests
    {
        public static void Run()
        {
            var initialCounter = new Counter { Count = 0 };

            var counterReducer =
                    BuildReducer
                        .For<Counter>()
                        .WhenActionHasType<Increment>((state, act) => new Counter { Count = state.Count + 1 })
                        .WhenActionHasType<Decrement>((state, act) => new Counter { Count = state.Count - 1 })
                        .WhenActionHasType<IncrementBy>((state, act) => new Counter { Count = state.Count + act.Value })
                        .Build();

            var store = Redux.CreateStore(counterReducer, initialCounter);

            QUnit.Module("Store");

            QUnit.Test("GetState() returns initial state when no dispatch has occured", assert =>
            {
                var result = store.GetState();
                assert.Equal(result.IsUndefined(), false);
                assert.Equal(result.Count, 0);
            });

            QUnit.Test("Dispatch() works", assert =>
            {
                store.Dispatch(new Increment());
                store.Dispatch(new IncrementBy { Value = 15 });
                var result = store.GetState();
                assert.Equal(result.Count, 16);
            });

            QUnit.Test("Dispatch() throws Exception when action is of type Object", assert =>
            {
                try
                {
                    var action = Script.Write<object>("{}");
                    store.Dispatch(action);
                }
                catch (Exception ex)
                {
                    assert.Equal(ex.IsUndefined(), false);
                    assert.Equal(string.IsNullOrWhiteSpace(ex.Message), false);
                }
            });

            QUnit.Test("Subscribe() works", assert =>
            {
                var outerCounter = 0;
                store.Subscribe(() =>
                {
                    outerCounter = outerCounter + 1;
                });

                store.Dispatch(new Increment());
                store.Dispatch(new Increment());
                store.Dispatch(new Increment());

                assert.Equal(outerCounter, 3);

                store.Dispatch(new IncrementBy { Value = 10 });

                assert.Equal(outerCounter, 4);

            });


        }
    }
}