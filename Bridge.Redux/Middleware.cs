using System;

namespace Bridge.Redux
{
    public class Middleware
    {
        public static Middleware From<TState>(Action<Store<TState>, dynamic, dynamic> func)
        {
            Func<Store<TState>, Func<dynamic, Action<dynamic>>> middleware =
                store =>
                {
                    return next =>
                    {
                        return action =>
                        {
                            func(store, next, action);
                        };
                    };
                };

            return Script.Write<Middleware>("middleware");
        }


        public static readonly Middleware Thunk = From<dynamic>((store, next, action) =>
        {
            /*@
            if (typeof action === 'function') { 
                return action(store.dispatch, store.getState);
            }

            next(action);

             */
        });
    }
}