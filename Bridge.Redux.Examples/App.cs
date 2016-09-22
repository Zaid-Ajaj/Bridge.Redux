
using Bridge.Html5;
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

            var todoListContainer = ReduxContainers.Create(new ContainerProps<IEnumerable<TodoItem>, IEnumerable<TodoItem>>
            {
                Store = store,
                StateToPropsMapper = state => state,
                Renderer = props => Components.TodoList(new TodoListProps
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