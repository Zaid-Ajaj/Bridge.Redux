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
        public extern void Dispatch<TActionType>(ReduxAction<TActionType> action);

        [Name("dispatch")]
        public extern void Dispatch<T, TActionType>(T action) where T : ReduxAction<TActionType>;

        [Name("dispatch")]
        public extern void Dispatch<TActionType>(Action<Action<ReduxAction<TActionType>>> dispatch);

        [Name("dispatch")]
        public extern void Dispatch<TActionType>(Action<Action<ReduxAction<TActionType>>, Func<TState>> dipatch);
    }
}