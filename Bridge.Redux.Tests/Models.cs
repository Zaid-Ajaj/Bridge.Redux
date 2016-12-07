using System;

namespace Bridge.Redux.Tests
{
    public class Increment { };
    [ObjectLiteral]
    public class IncrementBy { public int Value; }
    public class Decrement { };
    public class UnknownAction { }
    public class ClearName { };
    public class SetName { public string Value; }
    public class SetAge { public int Age; }
    public class SetGender { public Gender Gender; }
    [ObjectLiteral]
    public class Counter { public int Count { get; set; } }
    public enum Gender { Male, Female }
    
    [ObjectLiteral]
    public class CompoundState
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }

    

    

   

    

}