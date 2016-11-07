/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2016
 * @compiler Bridge.NET 15.3.0
 */
Bridge.assembly("ReactReduxCounter", function ($asm, globals) {
    "use strict";

    Bridge.define("ReactReduxCounter.App", {
        statics: {
            log: function (T, something) {
                console.log(something);
            }
        },
        $main: function () {
            var counterReducer = Bridge.Redux.BuildReducer.for(Object).whenStateIsUndefinedOrNull($_.ReactReduxCounter.App.f1).whenActionHasType$1(ReactReduxCounter.IncrementValue, $_.ReactReduxCounter.App.f2).whenActionHasType$1(ReactReduxCounter.DecrementValue, $_.ReactReduxCounter.App.f3).whenActionIsUnknown($_.ReactReduxCounter.App.f4).build();



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

            var initialState = { value: 0 };

            var store = Redux.createStore(counterReducer, initialState);

            store.subscribe(function () {
                Bridge.Console.log(System.String.format("Current value => {0}", store.getState().value));
            });
            Bridge.Redux.Extensions.dispatch(Object, ReactReduxCounter.IncrementValue, store, new ReactReduxCounter.IncrementValue());
            Bridge.Redux.Extensions.dispatch(Object, ReactReduxCounter.IncrementValue, store, new ReactReduxCounter.IncrementValue());
            Bridge.Redux.Extensions.dispatch(Object, ReactReduxCounter.IncrementValue, store, new ReactReduxCounter.IncrementValue());
            // Current value => 3

            var counterView = Bridge.ReactRedux.ReactRedux.component(Object, System.Int32, Bridge.merge(new (Bridge.ReactRedux.ContainerProps$2(Object,System.Int32))(), {
                setStore: store,
                setStateToPropsMapper: $_.ReactReduxCounter.App.f5,
                setRenderer: function (counterValue) {
                    return React.DOM.div({  }, React.DOM.button(ReactReduxCounter.Attr.onClick(function (e) {
                            Bridge.Redux.Extensions.dispatch(Object, ReactReduxCounter.IncrementValue, store, new ReactReduxCounter.IncrementValue());
                        }), "+"), React.DOM.h4(null, System.String.format("Counter value is {0}", counterValue)), React.DOM.button(ReactReduxCounter.Attr.onClick(function (e) {
                            Bridge.Redux.Extensions.dispatch(Object, ReactReduxCounter.DecrementValue, store, new ReactReduxCounter.DecrementValue());
                        }), "-"));
                }
            } ));


            ReactDOM.render(Bridge.React.Component$2(Bridge.ReactRedux.ContainerProps$2(Object,System.Int32),System.Int32).op_Implicit$1(counterView), document.getElementById("app"));
        }
    });

    var $_ = {};

    Bridge.ns("ReactReduxCounter.App", $_);

    Bridge.apply($_.ReactReduxCounter.App, {
        f1: function () {
            ReactReduxCounter.App.log(String, "State was either null or undefined, returning a default counter...");
            return { value: 0 };
        },
        f2: function (state, action) {
            return { value: ((state.value + 1) | 0) };
        },
        f3: function (state, action) {
            return { value: ((state.value - 1) | 0) };
        },
        f4: function (state) {
            ReactReduxCounter.App.log(String, "Action dispatched was unknown, returning the same state object back with no modifications");
            return state;
        },
        f5: function (counter) {
            return counter.value;
        }
    });

    Bridge.define("ReactReduxCounter.Attr", {
        statics: {
            onClick: function (ev) {
                return { onClick: ev };
            }
        }
    });

    Bridge.define("ReactReduxCounter.DecrementValue");

    Bridge.define("ReactReduxCounter.IncrementValue");

    Bridge.define("ReactReduxCounter.UnknownAction");
});
