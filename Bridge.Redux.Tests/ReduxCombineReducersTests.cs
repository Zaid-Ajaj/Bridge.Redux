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



            var nestedAppReducer = Redux.CombineReducers(new NestedAppState
            {
                Name = BuildReducer
                .For<string>()
                .WhenStateIsUndefinedOrNull(() => "Some cool app")
                .WhenActionHasType<SetAppStateName>((state, action) => action.Name)
                .Build(),

                Page = Redux.CombineReducers(new Page
                {
                    Title = BuildReducer
                             .For<string>()
                             .WhenStateIsUndefinedOrNull(() => "Home")
                             .WhenActionHasType<SetPageTitle>((oldTitle, action) => action.Title)
                             .Build(),

                    IsLoading = BuildReducer
                                .For<bool>()
                                .WhenStateIsUndefinedOrNull(() => false)
                                .WhenActionHasType<SetPageLoading>((state, action) => action.IsLoading)
                                .Build()
                }),

                User = Redux.CombineReducers(new User
                {
                    Name = BuildReducer
                            .For<string>()
                            .WhenStateIsUndefinedOrNull(() => "Zaid")
                            .WhenActionHasType<SetUserName>((state, action) => action.Name)
                            .Build(),

                    Settings = Redux.CombineReducers(new UserSettings
                    {
                        Role = BuildReducer
                                .For<UserRoles>()
                                .WhenStateIsUndefinedOrNull(() => UserRoles.Guest)
                                .WhenActionHasType<SetUserRole>((state, action) => action.Role)
                                .Build(),

                        AuthToken = BuildReducer
                                    .For<string>()
                                    .WhenStateIsUndefinedOrNull(() => "Auth")
                                    .WhenActionHasType<SetUserName>((state, action) => action.Name)
                                    .Build(),

                        LastLoggedIn = BuildReducer
                                        .For<string>()
                                        .WhenStateIsUndefinedOrNull(() => "Just now")
                                        .WhenActionHasType<SetLastLoggedIn>((state, action) => action.Date)
                                        .Build()
                    })
                })
            });

            var store = Redux.CreateStore(nestedAppReducer);

            QUnit.Module("Deeply nested reducers with CombineReducers");

            QUnit.Test("Nested Reducers as one reducer is defined", assert =>
            {
                assert.Equal(Script.IsDefined(nestedAppReducer), true);
            });

            QUnit.Test("Initial state is derived from reducers", assert =>
            {
                var initialState = store.GetState();

                assert.Equal(initialState.Name, "Some cool app");
                assert.Equal(initialState.User.Name, "Zaid");
                assert.Equal(initialState.User.Settings.Role, UserRoles.Guest);
                assert.Equal(initialState.User.Settings.LastLoggedIn, "Just now");
                assert.Equal(initialState.User.Settings.AuthToken, "Auth");
                assert.Equal(initialState.Page.IsLoading, false);
                assert.Equal(initialState.Page.Title, "Home");

            });

            QUnit.Test("Nested Reducers as one reducer works when dispachting an action to mutate grandchild", assert => 
            {
               
                store.Dispatch(new SetUserRole { Role = UserRoles.Admin });

                var nextUserRole = store.GetState().User.Settings.Role;

                assert.Equal(nextUserRole, UserRoles.Admin);
            });
        }
    }
}