namespace Bridge.Redux.Examples
{
    public class Actions
    {
        public static SimpleAction AddTodo(string text)
        {
            return new SimpleAction { Type = ActionTypes.AddTodo, TodoText = text };
        }

        public static SimpleAction ToggleTodo(int id)
        {
            return new SimpleAction { Type = ActionTypes.ToggleTodo, TodoId = id };
        }
    }
}