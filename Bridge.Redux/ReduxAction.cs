namespace Bridge.Redux
{
    [ObjectLiteral]
    [IgnoreGeneric]
    public class ReduxAction<T>
    {
        public T Type { get; set; }
    }
}