using Bridge;
using Bridge.Redux;
using Bridge.ReactRedux;
using Bridge.React;
using Bridge.Html5;

namespace ReactReduxCounter
{
    // Model
    [ObjectLiteral]
    class Counter
    {
        public int Value { get; set; }
    }

    enum ActionTypes
    {
        Increment,
        Decrement
    }

    static class Actions
    {
        public static ReduxAction<ActionTypes> Increment()
        {
            return new ReduxAction<ActionTypes> { Type = ActionTypes.Increment };
        }

        public static ReduxAction<ActionTypes> Decrement()
        {
            return new ReduxAction<ActionTypes> { Type = ActionTypes.Decrement };
        }
    }


    public static class App
    {
        public static void Main()
        {
            var counterReducer = ReduxReducers.Create((Counter state, ReduxAction<ActionTypes> action) =>
            {
                if (state.IsUndefined())
                    return new Counter { Value = 0 };

                if (action.Type == ActionTypes.Increment)
                {
                    return new Counter { Value = state.Value + 1 };
                } 
                else if (action.Type == ActionTypes.Decrement)
                {
                    return new Counter { Value = state.Value - 1 };
                }
                else
                {
                    return state;
                }
            });

            var store = Redux.CreateStore(counterReducer);

            var counterView = ReactRedux.Component(new ContainerProps<Counter, int>
            {
                Store = store,
                StateToPropsMapper = counter => counter.Value,
                Renderer = counteValue =>
                {
                    return DOM.Div(new Attributes { },
                             DOM.Button(new ButtonAttributes { OnClick = e => store.Dispatch(Actions.Increment()) }, "+"),
                             DOM.H4($"Counter value is {counteValue}"),
                             DOM.Button(new ButtonAttributes { OnClick = e => store.Dispatch(Actions.Decrement()) }, "-")
                          );
                }
            });


            React.Render(counterView, Document.GetElementById("app"));
        }
    }
}