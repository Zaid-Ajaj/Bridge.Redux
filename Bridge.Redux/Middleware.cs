using System;
using System.Linq;

namespace Bridge.Redux
{
    [ObjectLiteral]
    public class ReduxLoggerOuput<T>
    {
        public T StateBefore { get; set; }
        public T StateAfter { get; set; }
        public object ActionDispatched { get; set; }
    }
    public class ReduxMiddleware
    {
        public static ReduxMiddleware From<TState>(Action<Store<TState>, dynamic, dynamic> func)
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

            return Script.Write<ReduxMiddleware>("middleware");
        }


        public static readonly ReduxMiddleware Thunk = From<dynamic>((store, next, action) =>
        {
            /*@
            if (typeof action === 'function') { 
                return action(store.dispatch, store.getState);
            }

            next(action);

             */
        });

        /// <summary>
        /// Logs current state, then the action being dispatched, then the state after reduction
        /// </summary>
        public static readonly ReduxMiddleware DefaultLogger = From<dynamic>((store, next, action) =>
        {
            /*@
                console.log(store.getState());
                console.log(action);
                next(action)
                console.log(store.getState());
             */
        });

        public static ReduxMiddleware Logger<T>(Action<ReduxLoggerOuput<T>> logHandler)
        {
            /*@
             return Bridge.Redux.ReduxMiddleware.from(Object, function (store, dipatchNext, action) {
                var stateBefore = store.getState();
                dipatchNext(action);
                var stateAfter = store.getState();

                var loggerOutput = { 
                   stateBefore: stateBefore,
                   actionDispatched: action,
                   stateAfter: stateAfter
                };

                logHandler(loggerOutput);

             });
            */

            return Script.Write<ReduxMiddleware>("");
        }
    }
}