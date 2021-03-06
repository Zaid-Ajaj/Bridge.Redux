﻿namespace Bridge.Redux.Tests
{
    using QUnit;
    using System;

    public static class Helpers
    {
        public static bool IsUndefined<T>(this T value) => Script.Write<bool>("value === undefined");
    }
    public static class ReducerBuilderTests
    {
        public static void Run()
        {
            Func<int, Increment, int> incrCounter = (state, act) => state + 1;
            Func<int, Decrement, int> decrCounter = (state, act) => state - 1;
            Func<int, IncrementBy, int> inctByCounter = (state, act) => state + act.Value;


            var counterReducer =
                    BuildReducer
                        .For<int>()
                        .WhenActionHasType<Increment>(incrCounter)
                        .WhenActionHasType<Decrement>(decrCounter)
                        .WhenActionHasType<IncrementBy>(inctByCounter)
                        .WhenStateIsUndefinedOrNull(() => 10)
                        .WhenActionIsUnknown(state => state + 100)
                        .Build();

            //var store = Redux.CreateStore(counterReducer);

            QUnit.Module("ReduxReducer");

            QUnit.Test("ReduxReducer returned by reducer builder is defined", assert =>
            {
                assert.Equal(counterReducer.IsUndefined(), false);
            });

            QUnit.Test("WhenActionHasType<TAction>() works when action has no data", assert =>
            {
                var result = counterReducer.Apply(0, new Increment());
                assert.Equal(result, 1);
                var sndResult = counterReducer.Apply(1, new Decrement());
                assert.Equal(sndResult, 0);
            });


            QUnit.Test("WhenActionHasType<TAction>() throws ArgumentNullException when passed a null reducer", assert =>
            {
                try
                {
                    Func<int, Increment, int> nullReducer = null;
                    BuildReducer
                        .For<int>()
                        .WhenActionHasType<Increment>(incrCounter)
                        .WhenActionHasType<Decrement>(decrCounter)
                        .WhenActionHasType(nullReducer)
                        .WhenStateIsUndefinedOrNull(() => 10)
                        .WhenActionIsUnknown(state => state + 100)
                        .Build();
                }
                catch(Exception ex)
                {
                    assert.Equal(ex.GetType() == typeof(ArgumentNullException), true);
                }
            });

            QUnit.Test("WhenActionIsUnknown() throws ArgumentNullException when passed a null lambda", assert =>
            {
                try
                {
                    Func<int, int> nullReducer =  null;
                    BuildReducer
                        .For<int>()
                        .WhenActionHasType<Increment>(incrCounter)
                        .WhenActionHasType<Decrement>(decrCounter)
                        .WhenStateIsUndefinedOrNull(() => 10)
                        .WhenActionIsUnknown(nullReducer)
                        .Build();
                }
                catch (Exception ex)
                {
                    assert.Equal(ex.GetType() == typeof(ArgumentNullException), true);
                }
            });

            QUnit.Test("WhenStateIsUndefinedOrNull() throws ArgumentNullException when passed a null reducer", assert =>
            {
                try
                {
                    Func <int> nullLambda = null;
                    BuildReducer
                        .For<int>()
                        .WhenActionHasType<Increment>(incrCounter)
                        .WhenActionHasType<Decrement>(decrCounter)
                        .WhenStateIsUndefinedOrNull(nullLambda)
                        .Build();
                }
                catch (Exception ex)
                {
                    assert.Equal(ex.GetType() == typeof(ArgumentNullException), true);
                }
            });

            QUnit.Test("WhenActionHasType<TAction> works when action does have data", assert =>
            {
                var result = counterReducer.Apply(0, new IncrementBy { Value = 15 });
                assert.Equal(result, 15);
            });


            QUnit.Test("WhenActionIsUnknown works when dispatching unhandled action", assert =>
            {
                var result = counterReducer.Apply(10, new UnknownAction());
                assert.Equal(result, 110);
            });

            QUnit.Test("WhenStateIsUndefinedOrNull works when input is either null of undefined", assert =>
            {
                var nullValue = Script.Write<int>("null");
                var undefined = Script.Write<int>("undefined");

                var result = counterReducer.Apply(nullValue, new Increment());
                var sndResult = counterReducer.Apply(undefined, new Increment());

                assert.Equal(result, 10);
                assert.Equal(sndResult, 10);
            });
        }
    }
}