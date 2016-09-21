namespace Bridge.Redux.Examples.Basic
{
    [ObjectLiteral]
    public class AppState
    {
        public User User { get; set; }
        public Counter Counter { get; set; }
    }

    [ObjectLiteral]
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [ObjectLiteral]
    public class Counter
    {
        public int Value { get; set; }
    }

    public enum ActionTypes
    {
        Increment,
        Decrement,
        SetName,
        SetAge
    }

    public class SimpleAction : IReduxAction<ActionTypes>
    {
        public ActionTypes Type { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}