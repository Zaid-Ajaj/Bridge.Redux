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
        public extern void Dispatch<TActionType>(IReduxAction<TActionType> action);

        [Name("dispatch")]
        public extern void Dispatch<TActionType>(Action<Action<IReduxAction<TActionType>>> dispatch);

        [Name("dispatch")]
        public extern void Dispatch<TActionType>(Action<Action<IReduxAction<TActionType>>, Func<TState>> dipatch);
    }
}