using System;

namespace Bridge.Redux
{
    [External]
    public class Store<TState>
    {
        [Name("getState")]
        public extern TState GetState();

        [Name("subscribe")]
        public extern void Subscribe(Action action);

        [Name("dispatch")]
        public extern void Dispatch<T>(Action<Action<T>> dispatch);

        [Name("dispatch")]
        public extern void Dispatch<T>(Action<Action<T>, Func<TState>> dipatch);
    }
}