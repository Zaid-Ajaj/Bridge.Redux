using System.Collections.Generic;
using Bridge.Html5;

namespace Bridge.Redux.Examples
{
    public class ReactTodoApp
    {
        public static void Init()
        {
            var appReducer = Reducers.ItemsReducer();
            var initialState = new TodoItem[] { };
            var store = Redux.CreateStore(appReducer, initialState);

            React.React.Render(
                    ReactRedux.Provider(store, Containers.TodoApp()),
                    Document.GetElementById("app")
                  );
        }
    }
}