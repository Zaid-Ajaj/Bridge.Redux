using Bridge.Html5;
using Bridge.React;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bridge.Redux.Examples
{
    public class Components
    {
        public static ReactElement Todo(TodoItem item)
        {
            Func<TodoItem, ReactElement> renderer = todoItem =>
            {
                var attrs = new Attributes { Style = todoItem.IsDone ? Styles.Done : Styles.NotDone };
                return DOM.Div(attrs, todoItem.Text);
            };

           

            return StaticComponent.Stateless(renderer, item);
        }


        public static ReactElement TodoList(TodoListProps props)
        {
            Func<TodoListProps, ReactElement> renderer = todoListProps =>
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

                                   return DOM.Li(attrs, Todo(todo));
                               })
                           )
                       );
            };

            return StaticComponent.Stateless(renderer, props);
        }
    }
}