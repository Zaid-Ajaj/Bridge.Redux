namespace Bridge.Redux.Examples.Basic
{
    public static class Actions
    {
        public static SimpleAction Increment()
        {
            return new SimpleAction { Type = ActionTypes.Increment };
        }

        public static SimpleAction Decrement()
        {
            return new SimpleAction { Type = ActionTypes.Decrement };
        }

        public static SimpleAction SetUserName(string name)
        {
            return new SimpleAction { Type = ActionTypes.SetName, Name = name };
        }

        public static SimpleAction SetUserAge(int age)
        {
            return new SimpleAction { Type = ActionTypes.SetAge, Age = age };
        }
    }
}