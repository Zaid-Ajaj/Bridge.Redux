using Bridge;
using Bridge.Redux;
using Bridge.ReactRedux;
using Bridge.React;
using Bridge.Html5;
using System;

namespace ReactReduxCounter
{
    // State
    [ObjectLiteral]
    class Counter
    {
        public int Value { get; set; }
    }

    // Actions 
    public class IncrementValue { }
    public class DecrementValue { }

    public class UnknownAction { };
     

    // Simpler "idiomatic" actions
    // ===============================
    //public enum ActionTypes
    //{
    //    Increment, Decrement
    //}

    //[ObjectLiteral]
    //public class CounterAction
    //{
    //    public ActionTypes Type { get; set; }
    //}

   
    static class Attr
    {
        public static ButtonAttributes OnClick(Action<Bridge.React.MouseEvent<HTMLButtonElement>> ev)
        {
            return new ButtonAttributes { OnClick = ev };
        }
    }
    public static class App
    {
        static void Log<T>(T something)
        {
            Script.Write("console.log(something)");
        }
        public static void Main()
        {
            var counterReducer = 
                  BuildReducer
                    .For<Counter>() 
                    .WhenStateIsUndefinedOrNull(() =>
                    {
                        Log("State was either null or undefined, returning a default counter...");
                        return new Counter { Value = 0 };
                    })
                    .WhenActionHasType<IncrementValue>((state, action) =>
                    {
                        return new Counter { Value = state.Value + 1 };
                    }) 
                    .WhenActionHasType<DecrementValue>((state, action) =>
                    {
                        return new Counter { Value = state.Value - 1 };
                    })
                    .WhenActionIsUnknown(state =>
                    {
                        Log("Action dispatched was unknown, returning the same state object back with no modifications");
                        return state;
                    })
                    .Build();



            //var counterReducer = ReduxReducers.Create((Counter state, CounterAction action) =>
            //{
            //    if (state.IsUndefined())
            //        return new Counter { Value = 0 };

            //    if (action.Type == ActionTypes.Increment)
            //    {
            //        return new Counter { Value = state.Value + 1 };
            //    }
            //    else if (action.Type == ActionTypes.Decrement)
            //    {
            //        return new Counter { Value = state.Value - 1 };
            //    }
            //    else
            //    {
            //        return state;
            //    }
            //});

            var initialState = new Counter { Value = 0 };

            var store = Redux.CreateStore(counterReducer, initialState);

            store.Subscribe(() => Console.WriteLine($"Current value => {store.GetState().Value}"));

            store.Dispatch(new IncrementValue());
            // Current value => 1
            store.Dispatch(new IncrementValue());
            // Current value => 2
            store.Dispatch(new IncrementValue());
            // Current value => 3

            var counterView = ReactRedux.Component(new ContainerProps<Counter, int>
            {
                Store = store,
                StateToPropsMapper = counter => counter.Value,
                Renderer = counterValue =>
                {
                    return DOM.Div(new Attributes { },
                             DOM.Button(Attr.OnClick(e => store.Dispatch(new IncrementValue())), "+"),
                             DOM.H4($"Counter value is {counterValue}"),
                             DOM.Button(Attr.OnClick(e => store.Dispatch(new DecrementValue())), "-")
                          );
                }
            });


            React.Render(counterView, Document.GetElementById("app"));
        }
    }
}