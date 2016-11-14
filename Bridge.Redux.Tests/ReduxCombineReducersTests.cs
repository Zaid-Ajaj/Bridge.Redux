namespace Bridge.Redux.Tests
{
    using QUnit;



    public static class ReduxCombineReducersTests
    {
        public static void Run()
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
                assert.Equal(result1.IsUndefined(), false);
                assert.Equal(result1.Name, "Sanne"); // only the name is changed
                assert.Equal(result1.Gender, Gender.Male); // same as initial
                assert.Equal(result1.Age, 19); // same as initial

                var result2 = complexReducer.Apply(result1, new SetAge { Age = 18 });
                assert.Equal(result2.IsUndefined(), false);
                assert.Equal(result2.Name, "Sanne");
                assert.Equal(result2.Gender, Gender.Male);
                assert.Equal(result2.Age, 18);

                var result3 = complexReducer.Apply(result2, new SetGender { Gender = Gender.Female });
                assert.Equal(result3.IsUndefined(), false);
                assert.Equal(result3.Name, "Sanne");
                assert.Equal(result3.Gender, Gender.Female);
                assert.Equal(result3.Age, 18);
            });
        }
    }
}