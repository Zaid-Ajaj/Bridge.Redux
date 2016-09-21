using Bridge.React;
using Bridge.Redux;
using System;

namespace Bridge.Redux.Examples
{
    public class ReactRedux
    {
        public static ReactElement Provider<T>(Store<T> reduxStore, ReactElement mainApp)
        {
            return Script.Write<ReactElement>("React.createElement(ReactRedux.Provider, { store: reduxStore }, mainApp)");
        }

        [Template("ReactRedux.connect({0}, {1})")]
        public static extern Func<Func<TProps, ReactElement>, ReactElement> Connect<TState, TProps>(Func<TState, TProps> mapStateToProps, Func<Action<SimpleAction>, TProps> mapDispatchToProps);

    }
}