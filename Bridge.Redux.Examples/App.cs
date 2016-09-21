
using Bridge.Html5;
using System;

namespace Bridge.Redux.Examples
{
    public class App
    {
        public static void Main()
        {
            var appReducer = Reducers.ItemsReducer();
            var initialState = new TodoItem[] { };
            var store = Redux.CreateStore(appReducer, initialState);

            Action render = () =>
            {
                var todoList = Components.TodoList(new TodoListProps
                {
                    Todos = store.GetState(),
                    AddTodo = text => store.Dispatch(Actions.AddTodo(text)),
                    ToggleTodo = id => store.Dispatch(Actions.ToggleTodo(id))
                });

                var appContainer = Document.GetElementById("app");

                React.React.Render(todoList, appContainer);
            };

            store.Subscribe(render);

            render();
        }
    }
}