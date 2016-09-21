using Bridge.React;
using System;
using System.Collections.Generic;

namespace Bridge.Redux.Examples
{
    public class Containers
    {
        public static ReactElement TodoApp()
        {
            Func<IEnumerable<TodoItem>, TodoListProps> mapStateToProps = state => new TodoListProps
            {
                Todos = state
            };

            Func<Action<SimpleAction>, TodoListProps> mapDispatchToProps = dispatch => new TodoListProps
            {
                AddTodo = text => dispatch(Actions.AddTodo(text)),
                ToggleTodo = id => dispatch(Actions.ToggleTodo(id))
            };

            var connecter = ReactRedux.Connect(mapStateToProps, mapDispatchToProps);

            return connecter(Components.TodoList);
        }
    }
}