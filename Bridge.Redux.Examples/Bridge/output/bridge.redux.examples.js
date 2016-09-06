﻿(function (globals) {
    "use strict";

    Bridge.define('Bridge.Redux.Examples.Actions', {
        statics: {
            increment: function () {
                return { type: Bridge.Redux.Examples.ActionTypes.Increment };
            },
            decrement: function () {
                return { type: Bridge.Redux.Examples.ActionTypes.Decrement };
            },
            setUserName: function (name) {
                return { type: Bridge.Redux.Examples.ActionTypes.SetName, name: name };
            },
            setUserAge: function (age) {
                return { type: Bridge.Redux.Examples.ActionTypes.SetAge, age: age };
            }
        }
    });
    
    Bridge.define('Bridge.Redux.Examples.ActionTypes', {
        statics: {
            Increment: 0,
            Decrement: 1,
            SetName: 2,
            SetAge: 3
        },
        $enum: true
    });
    
    Bridge.define('Bridge.Redux.Examples.App', {
        statics: {
            config: {
                init: function () {
                    Bridge.ready(this.main);
                }
            },
            main: function () {
                var appReducer = Redux.combineReducers({ user: Bridge.Redux.Reducer$1.op_Implicit(Bridge.Redux.Examples.Reducers.userReducer()), counter: Bridge.Redux.Reducer$1.op_Implicit(Bridge.Redux.Examples.Reducers.counterReducer()) });
    
                var middleware = Redux.applyMiddleware(Bridge.Redux.Middleware.thunk);
    
                var store = Redux.createStore(appReducer, middleware);
    
                store.subscribe(function () {
                    System.Console.log(store.getState());
                });
    
                store.dispatch(Bridge.Redux.Examples.Actions.increment());
                store.dispatch(Bridge.Redux.Examples.Actions.increment());
                store.dispatch(Bridge.Redux.Examples.Actions.decrement());
                store.dispatch(Bridge.Redux.Examples.Actions.setUserName("Zaid"));
                store.dispatch(Bridge.Redux.Examples.Actions.setUserAge(19));
    
                store.dispatch($_.Bridge.Redux.Examples.App.f1);
    
                store.dispatch($_.Bridge.Redux.Examples.App.f2);
            }
        },
        $entryPoint: true
    });
    
    var $_ = {};
    
    Bridge.ns("Bridge.Redux.Examples.App", $_);
    
    Bridge.apply($_.Bridge.Redux.Examples.App, {
        f1: function (dispatch) {
            System.Console.log("Beginning 'async' operation");
            window.setTimeout(function () {
                System.Console.log("Dispatching 'Increment'");
                dispatch(Bridge.Redux.Examples.Actions.increment());
            }, 5000);
        },
        f2: function (dispatch, getState) {
            System.Console.log("Beginning 'async' operation");
            window.setTimeout(function () {
                System.Console.log(getState());
                System.Console.log("Dispatching 'Increment'");
                dispatch(Bridge.Redux.Examples.Actions.increment());
            }, 5000);
        }
    });
    
    Bridge.define('Bridge.Redux.Examples.Reducers', {
        statics: {
            counterReducer: function () {
                return Bridge.Redux.Reducer$1.create$1($_.Bridge.Redux.Examples.Reducers.f1);
            },
            userReducer: function () {
                return Bridge.Redux.Reducer$1.create$1($_.Bridge.Redux.Examples.Reducers.f2);
            }
        }
    });
    
    Bridge.ns("Bridge.Redux.Examples.Reducers", $_);
    
    Bridge.apply($_.Bridge.Redux.Examples.Reducers, {
        f1: function (state, action) {
            if (Bridge.Redux.Extensions.isUndefined(Object, state)) {
                return { value: 0 };
            }
    
            if (action.type === Bridge.Redux.Examples.ActionTypes.Increment) {
                return { value: ((state.value + 1) | 0) };
            }
    
            if (action.type === Bridge.Redux.Examples.ActionTypes.Decrement) {
                return { value: ((state.value - 1) | 0) };
            }
    
            return state;
        },
        f2: function (state, action) {
            if (Bridge.Redux.Extensions.isUndefined(Object, state)) {
                return {  };
            }
    
            if (action.type === Bridge.Redux.Examples.ActionTypes.SetName) {
                return { name: action.name, age: state.age };
            }
    
            if (action.type === Bridge.Redux.Examples.ActionTypes.SetAge) {
                return { name: state.name, age: action.age };
            }
    
            return state;
        }
    });
    
    Bridge.init();
})(this);
