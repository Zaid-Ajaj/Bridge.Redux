namespace Bridge.Redux.Tests
{

    [Enum(Emit.StringName)]
    public enum UserRoles { Guest, Admin }


    [ObjectLiteral]
    public class UserSettings
    {
        public UserRoles Role = UserRoles.Guest;
        public string AuthToken;
        public string LastLoggedIn;
    }

    [ObjectLiteral]
    public class User
    {
        public string Name { get; set; }
        public UserSettings Settings { get; set; }
    }
}