﻿Bridge.assembly("Bridge.Redux", function ($asm, globals) {
    "use strict";

    Bridge.define("Bridge.Redux.Extensions", {
        statics: {
            isUndefined: function (T, value) {
                return value === undefined;
            },
            mergeWith: function (T, value, otherValue) {
                var result = { };
            
                for (var key in value) { 
                    if (typeof value[key] === 'function') {
                        continue;
                    }
                
                    result[key] = value[key];
            
                }
               for (var key in otherValue) { 
                    if (typeof otherValue[key] === 'function') {
                        continue;
                    }
                
                    result[key] = otherValue[key];
            
                }
             
                return result;
            }
        }
    });

    Bridge.definei("Bridge.Redux.IReduxAction$1", {
        $kind: "interface"
    });

    Bridge.define("Bridge.Redux.Middleware", {
        statics: {
            thunk: null,
            config: {
                init: function () {
                    this.thunk = Bridge.Redux.Middleware.from(Object, $_.Bridge.Redux.Middleware.f1);
                }
            },
            from: function (TState, func) {
                var middleware = function (store) {
                    return function (next) {
                        return function (action) {
                            func(store, next, action);
                        };
                    };
                };

                return middleware;
            }
        }
    });

    var $_ = {};

    Bridge.ns("Bridge.Redux.Middleware", $_);

    Bridge.apply($_.Bridge.Redux.Middleware, {
        f1: function (store, next, action) {
        
                if (typeof action === 'function') { 
                    return action(store.dispatch, store.getState);
                }

                next(action);

             
        }
    });

    Bridge.define("Bridge.Redux.Reducer$1", {
        statics: {
            create: function (reducer) {
                return reducer;
            },
            create$1: function (reducer) {
                return reducer;
            },
            op_Implicit: function (reducer) {
                return reducer;
            }
        },
        ctor: function () {
            this.$initialize();
        }
    });
});
