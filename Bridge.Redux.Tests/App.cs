using System;
using Bridge.Redux;


namespace Bridge.Redux.Tests
{
    using QUnit;

    public class App
    {
        private class Increment { };
        private class IncrementBy { public int Value; }
        private class Decrement { };
        private class UnknownAction { }

        private class ClearName { };
        private class SetName { public string Value; }
        private class SetAge { public int Age;  }
        private class SetGender { public Gender Gender; }

        private enum Gender { Male, Female }

        [ObjectLiteral]
        private class CompoundState
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Gender Gender { get; set; }
        }

        public static void ReducerBuilderTestsForPrimitiveState()
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

            QUnit.Test("Reducer is not  undefined", assert =>
            {
                assert.Equal(counterReducer.IsUndefined(), false);
            });

            QUnit.Test("WhenActionHasType<TAction> works when action has no data", assert =>
            {
                var result = counterReducer.Apply(0, new Increment());
                assert.Equal(result, 1);
                var sndResult = counterReducer.Apply(1, new Decrement());
                assert.Equal(sndResult, 0);
            });

            QUnit.Test("WhenActionHasType<TAction> works when action does have data", assert =>
            {
                var result = counterReducer.Apply(0, new IncrementBy { Value = 15 });
                assert.Equal(result, 15);
            });


            QUnit.Test("WhenActionIsUnknown", assert =>
            {
                var result = counterReducer.Apply(10, new UnknownAction());
                assert.Equal(result, 110);
            });

            QUnit.Test("WhenStateIsUndefinedOrNull works", assert =>
            {
                var nullValue = Script.Write<int>("null");
                var undefined = Script.Write<int>("undefined");

                var result = counterReducer.Apply(nullValue, new Increment());
                var sndResult = counterReducer.Apply(undefined, new Increment());

                assert.Equal(result, 10);
                assert.Equal(sndResult, 10);
            });
        }

        [IgnoreGeneric]
        private static void Log<T>(T obj)
        {
            Script.Write("console.log(obj)");
        }

        public static void ReduxReducerTestsForComplexState()
        {
            var nameReducer =
                    BuildReducer
                        .For<string>()
                        .WhenStateIsUndefinedOrNull(() => "")
                        .WhenActionIsUnknown(state => state)
                        .WhenActionHasType<SetName>((state, act) => act.Value)
                        .Build();

            var ageReducer =
                    BuildReducer
                        .For<int>()
                        .WhenStateIsUndefinedOrNull(() => 0)
                        .WhenActionIsUnknown(state => state)
                        .WhenActionHasType<SetAge>((state, act) => act.Age)
                        .Build();

            var genderReducer =
                    BuildReducer
                        .For<Gender>()
                        .WhenStateIsUndefinedOrNull(() => Gender.Male)
                        .WhenActionIsUnknown(state => state)
                        .WhenActionHasType<SetGender>((state, act) => act.Gender)
                        .Build();

            var complexReducer = Redux.CombineReducers(new CompoundState
            {
                Name = nameReducer,
                Age = ageReducer,
                Gender = genderReducer
            });

            QUnit.Module("Redux.CombineReducers()");

            QUnit.Test("Resulting reducer from Redux.CombineRuducers is defined", assert =>
            {
                assert.Equal(complexReducer.IsUndefined(), false);
            });

            

            QUnit.Test("Resulting reducer from Redux.CombineRuducers works", assert =>
            {
                var oldState = new CompoundState
                {
                    Name = "Zaid",
                    Age = 19,
                    Gender = Gender.Male
                };

                var result1 = complexReducer.Apply(oldState, new SetName { Value = "Sanne" });
                var result2 = complexReducer.Apply(result1, new SetAge { Age = 18 });
                var result3 = complexReducer.Apply(result2, new SetGender { Gender = Gender.Female });
            
                assert.Equal(result1.IsUndefined(), false);
                assert.Equal(result1.Name, "Sanne"); // only the name is changed
                assert.Equal(result1.Gender, Gender.Male); // same as initial
                assert.Equal(result1.Age, 19); // same as initial


                assert.Equal(result2.IsUndefined(), false);
                assert.Equal(result2.Name, "Sanne");
                assert.Equal(result2.Gender, Gender.Male); 
                assert.Equal(result2.Age, 18); 

                assert.Equal(result3.IsUndefined(), false);
                assert.Equal(result3.Name, "Sanne");
                assert.Equal(result3.Gender, Gender.Female);
                assert.Equal(result3.Age, 18);
            });
        }

        [ObjectLiteral]
        private class Counter { public int Count { get; set; } }

        public static void ReduxStoreWorks()
        {
            var initialCounter = new Counter { Count = 0 };

            var counterReducer =
                BuildReducer.For<Counter>()
                            .WhenActionHasType<Increment>((state, act) => new Counter { Count = state.Count + 1 })
                            .WhenActionHasType<Decrement>((state, act) => new Counter { Count = state.Count - 1 })
                            .WhenActionHasType<IncrementBy>((state, act) => new Counter { Count = state.Count + act.Value })
                            .Build();

            var store = Redux.CreateStore(counterReducer, initialCounter);

            QUnit.Module("Store");

            QUnit.Test("GetState() from store get passed initial state when no dispatch has occured", assert =>
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

        public static void Main()
        {
            ReducerBuilderTestsForPrimitiveState();
            ReduxReducerTestsForComplexState();
            ReduxStoreWorks();
        }
    }
}
