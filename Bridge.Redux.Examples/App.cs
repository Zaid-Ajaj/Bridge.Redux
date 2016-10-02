using Bridge.Html5;
using Bridge.React;
using Bridge.ReactRedux;
using System;
using System.Collections.Generic;

namespace Bridge.Redux.Examples
{
    public class App
    {
        public static void Main()
        {
            var appReducer = AppReducers.ItemsReducer();

            var initialState = new TodoItem[] { };
            var store = Redux.CreateStore(appReducer, initialState);

            var todoListContainer = ReactRedux.ReactRedux.Component(new ContainerProps<IEnumerable<TodoItem>, IEnumerable<TodoItem>>
            {
                Store = store,
                StateToPropsMapper = state => state,
                Renderer = props => StaticComponent.Stateless(Components.TodoList, new TodoListProps
                {
                    Todos = props,
                    AddTodo = text => store.Dispatch(Actions.AddTodo(text)),
                    ToggleTodo = id => store.Dispatch(Actions.ToggleTodo(id))
                })
            });

            React.React.Render(
                    todoListContainer,
                    Document.GetElementById("app")
            );
        }
    }
}