# Bridge.Redux - [Wiki](https://github.com/Zaid-Ajaj/Bridge.Redux/wiki)
Bindings of the Redux library for the Bridge transpiler. It provides a type-safe interface to interact with the redux library as well as DSL's to idiomatically create reducers from C#. 

# Installation
```
Install-Package Bridge.Redux
````
# Usage 
First define your state object, in our case it is a simple counter
```csharp
// State (object), it should to be a simple object literal that only holds data
// its properties however, could be anything. 
[ObjectLiteral]
class Counter
{
    public int Value { get; set; }
}
```
Then define the actions upon which the reduction is based
```csharp
// Actions
public class IncrementValue { }
public class DecrementValue { }

// Use fields instead of properties because properties will be translated to getter and setter methods
public class UpdateTodo 
{
    public int Id;
    public string Description;
}
```
Now for actually managing the state, lets create a counter reducer, Bridge.Redux provides a clean DSL to compose lambda's into a reducer based on the types of actions
```csharp
var counterReducer = 
    BuildReducer
      .For<Counter>() 
      .WhenActionHasType<IncrementValue>((state, action) =>
      {
          return new Counter { Value = state.Value + 1 };
      }) 
      .WhenActionHasType<DecrementValue>((state, action) =>
      {
          return new Counter { Value = state.Value - 1 };
      })
      .Build();
```
NOTE: 
    Redux actions MUST NOT be `[ObjectLiteral]` when using the above `BuildReducer` class because the generic type information would disappear during run-time while it is needed for the DSL to infer what method should be executed.

Create your store
```csharp
var initialState = new Counter { Value = 0 };
var store = Redux.CreateStore(counterReducer, initialState);
```
Subscribe (listen) to store dispatches
```csharp
store.Subscribe(() => Console.WriteLine($"Current value => {store.GetState().Value}"));
```
Finally dispatch actions
```csharp
store.Dispatch(new IncrementValue());
// Current value => 1
store.Dispatch(new IncrementValue());
// Current value => 2
store.Dispatch(new IncrementValue());
// Current value => 3
```
There you have it, a working redux app, utilizing C#'s type-system :)
### A more involved demo project is the [ReactReduxTodoApp](https://github.com/Zaid-Ajaj/ReactReduxTodoApp)

Wait, there is more...

# Usage with Bridge.React
First install the ReactRedux library, this library depends on `Bridge.React` and `Bridge.Redux`
```
Install-Package Bridge.ReactRedux
```
The Bridge.ReactRedux library integrates Bridge.Redux with Bridge.React to enable writing React + Redux apps in pure C#. using it is very simple:
- Provide a `Store<TState>` (Store)
- Provide a `Func<TState, TProps>` (StateToPropsMapper)
- Provide a `Func<TProps, ReactElement>` Renderer
```csharp
var counterView = ReactRedux.Component(new ContainerProps<Counter, int>
{
    Store = store,
    StateToPropsMapper = counter => counter.Value,
    Renderer = counterValue =>
    {
        return DOM.Div(new Attributes { },
                 DOM.Button(new ButtonAttributes { OnClick = e => store.Dispatch(new IncrementValue()) }, "+"),
                 DOM.H4($"Counter value is {counterValue}"),
                 DOM.Button(new ButtonAttributes { OnClick = e => store.Dispatch(new DecrementValue()) }, "-")
              );
    }
});
```
Render the component
```csharp
React.Render(counterView, Document.GetElementById("app"));
```
