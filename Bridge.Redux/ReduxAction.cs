namespace Bridge.Redux
{
    [ObjectLiteral]
    public class ReduxAction<T>
    {
        public T Type { get; set; }
    }
}