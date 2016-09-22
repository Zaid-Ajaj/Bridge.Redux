using System;
using System.Collections.Generic;

namespace Bridge.Redux.Examples
{
    [ObjectLiteral]
    public class TodoItem
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string Text { get; set; }
    }

    [ObjectLiteral]
    public class TodoListProps
    {
        public IEnumerable<TodoItem> Todos { get; set; }
        public Action<string> AddTodo { get; set; }
        public Action<int> ToggleTodo { get; set; }
    }

    [ObjectLiteral]
    public class AppState
    {
        public IEnumerable<TodoItem> Items { get; set; }
    }

    public enum ActionTypes
    {
        AddTodo,
        ToggleTodo
    }

    [ObjectLiteral]
    public class SimpleAction : ReduxAction<ActionTypes>
    {
        public string TodoText { get; set; }
        public int TodoId { get; set; }
    }
}