namespace Bridge.Redux
{
    [IgnoreGeneric]
    public interface IReduxAction<T>
    {
        T Type { get; set; }
    }
}