using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Redux
{
    [Name("Redux")]
    [External]
    public static class Redux
    {
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer);
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer, TState initialState);
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer, StoreMiddleware middleware);
        [Name("createStore")]
        public static extern Store<TState> CreateStore<TState>(ReduxReducer<TState> reducer, TState initialState, StoreMiddleware middleware);
        [Name("applyMiddleware")]
        public static extern StoreMiddleware ApplyMiddleware(params Middleware[] middlewares);
        [Name("combineReducers")]
        public static extern ReduxReducer<TState> CombineReducers<TState>(TState state);
    }
}
