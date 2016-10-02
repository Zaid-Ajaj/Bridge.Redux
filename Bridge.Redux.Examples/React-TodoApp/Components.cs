using Bridge.Html5;
using Bridge.React;
using System.Linq;

namespace Bridge.Redux.Examples
{
    public class Components
    {
        [Name("TodoItem")]
        public static ReactElement TodoItem(TodoItem todo)
        {
            var attrs = new Attributes { Style = todo.IsDone ? Styles.Done : Styles.NotDone };
            return DOM.Div(attrs, todo.Text);
        }

        [Name("TodoList")]
        public static ReactElement TodoList(TodoListProps todoListProps)
        {
            return DOM.Div
            (new Attributes { },
                DOM.Input(new InputAttributes
                {
                    Type = InputType.Text,
                    Placeholder = "Add todo",
                    OnKeyDown = e =>
                    {
                        var textInput = e.CurrentTarget.Value;
                        if (e.Which == 13 && textInput.Length > 0)
                        {
                            todoListProps.AddTodo(textInput);
                            e.CurrentTarget.Value = "";
                        }
                    }
                }),
                DOM.UL(new Attributes { },
                    todoListProps.Todos.Select(todo =>
                    {
                        var attrs = new LIAttributes
                        {
                            Key = todo.Id,
                            OnClick = e => todoListProps.ToggleTodo(todo.Id)
                        };

                        return DOM.Li(attrs, StaticComponent.Pure(TodoItem, todo));
                    })
                )
            );
        }
    }
}