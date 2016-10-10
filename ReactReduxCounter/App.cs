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

   

    public static class App
    {
        static class Pure
        {
            public static Counter Increment(Counter state, IncrementValue action)
            {
                return new Counter { Value = state.Value + 1 };
            }

            public static Counter Decrement(Counter state, DecrementValue action)
            {
                return new Counter { Value = state.Value - 1 };
            }
        }
        


        public static void Main()
        {
            var counterReducer = 
                BuildReducer
                  .For<Counter>()
                  .WhenActionHasType<IncrementValue>(state =>
                  {
                      return new Counter { Value = state.Value + 1 };
                  }) 
                  .WhenActionHasType<DecrementValue>((state, action) =>
                  {
                      return new Counter { Value = state.Value - 1 };
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
                Renderer = counteValue =>
                {
                    return DOM.Div(new Attributes { },
                             DOM.Button(new ButtonAttributes { OnClick = e => store.Dispatch(new IncrementValue()) }, "+"),
                             DOM.H4($"Counter value is {counteValue}"),
                             DOM.Button(new ButtonAttributes { OnClick = e => store.Dispatch(new DecrementValue()) }, "-")
                          );
                }
            });


            React.Render(counterView, Document.GetElementById("app"));
        }
    }
}