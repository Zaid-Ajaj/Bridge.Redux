namespace Bridge.Redux.Tests
{
    public class App
    {
        public static void Main()
        {
            ReducerBuilderTests.Run();
            ReduxCombineReducersTests.Run();
            ReduxStoreTests.Run();
            ReduxThunkTests.Run();
            LoggerMiddlewareTests.Run();
        }
    }
}
