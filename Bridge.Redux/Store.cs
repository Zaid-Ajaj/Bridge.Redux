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
        public extern void ThunkDispatch(Action<Action<object>> dispatch);

        [Name("dispatch")]
        public extern void ThunkDispatch(Action<Action<object>, Func<TState>> dipatch);
    }
}