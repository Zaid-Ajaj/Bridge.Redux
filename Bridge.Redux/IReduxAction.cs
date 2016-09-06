namespace Bridge.Redux
{
    [IgnoreGeneric]
    [ObjectLiteral]
    public interface IReduxAction<T>
    {
        T Type { get; set; }
    }
}